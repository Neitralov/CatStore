namespace Domain.Services;

public class UserService(IUserRepository userRepository, IConfiguration configuration)
{
    private const int AccessTokenLifeTimeInMinutes = 15;

    public async Task<ErrorOr<Created>> StoreUser(User user)
    {
        if (await userRepository.IsUserExists(user.Email))
            return Errors.User.AlreadyExists;

        await userRepository.AddUser(user);
        await userRepository.SaveChanges();

        return Result.Created;
    }

    public async Task<ErrorOr<Updated>> ChangeUserPassword(
        Guid userId,
        string oldPassword,
        string newPassword,
        string confirmNewPassword)
    {
        var user = await userRepository.FindUserById(userId);

        if (user is null)
            return Errors.User.NotFound;

        if (VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt) is false)
            return Errors.User.IncorrectOldPassword;

        if (newPassword != confirmNewPassword)
            return Errors.User.PasswordsDontMatch;

        if (newPassword == oldPassword)
            return Errors.User.NewAndOldPasswordAreTheSame;

        var result = user.ChangePassword(newPassword);

        if (result == Result.Updated)
            await userRepository.SaveChanges();

        return result;
    }

    public async Task<ErrorOr<TokensPair>> RefreshTokens(string? expiredAccessToken, string? refreshToken)
    {
        if (expiredAccessToken is null)
            return Errors.AccessToken.NotFound;

        if (refreshToken is null)
            return Errors.RefreshToken.NotFound;

        var claimsPrincipal = GetPrincipal(expiredAccessToken);

        if (claimsPrincipal.IsError)
            return claimsPrincipal.Errors;

        var userEmail = claimsPrincipal.Value.Identity?.Name!;

        var user = await userRepository.FindUserByEmail(userEmail);

        if (user is null)
            return Errors.User.NotFound;

        var refreshTokenSession = await userRepository.GetUserRefreshTokenSession(user.UserId, refreshToken);

        if (refreshTokenSession is null)
            return Errors.RefreshToken.TokenIsInvalid;

        refreshTokenSession.Update();
        await userRepository.SaveChanges();

        var newAccessToken = CreateAccessToken(user);
        var newRefreshToken = refreshTokenSession.Token;

        return (newAccessToken, newRefreshToken);
    }

    public async Task<ErrorOr<TokensPair>> Login(string email, string password)
    {
        var user = await userRepository.FindUserByEmail(email);

        if (user is null)
            return Errors.Login.IncorrectEmailOrPassword;

        if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) is false)
            return Errors.Login.IncorrectEmailOrPassword;

        var refreshTokenSession = RefreshTokenSession.Create(user.UserId);

        if (await AreThereTooManySessionsPerUser(refreshTokenSession.UserId))
            await userRepository.RemoveAllUsersRefreshTokenSessions(refreshTokenSession.UserId);

        await userRepository.AddRefreshTokenSession(refreshTokenSession);
        await userRepository.SaveChanges();

        var accessToken = CreateAccessToken(user);
        var refreshToken = refreshTokenSession.Token;

        return (accessToken, refreshToken);
    }

    private string CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new (ClaimTypes.Name, user.Email),
            new (nameof(user.CanEditCats), user.CanEditCats.ToString())
        };

        var configToken = configuration["AppSettings:Token"] ?? throw new NullReferenceException();
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configToken));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AccessTokenLifeTimeInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ErrorOr<ClaimsPrincipal> GetPrincipal(string accessToken)
    {
        var configToken = configuration["AppSettings:Token"] ?? throw new NullReferenceException();
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configToken));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            return tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _);
        }
        catch
        {
            return Errors.AccessToken.TokenIsInvalid;
        }
    }

    private static bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private async Task<bool> AreThereTooManySessionsPerUser(Guid userId)
    {
        return await userRepository.GetNumberOfUsersRefreshTokenSessions(userId) >= RefreshTokenSession.MaxRefreshTokenSessionsPerUser;
    }
}