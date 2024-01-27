namespace Domain.Services;

public class UserService(IUserRepository userRepository, IConfiguration configuration)
{
    public ErrorOr<Created> StoreUser(User user)
    {
        if (IsUserExists(user.Email))
            return Errors.User.AlreadyExists;

        userRepository.AddUser(user);
        userRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<string> Login(string email, string password)
    {
        var user = userRepository.FindUserByEmail(email);

        if (user is null)
            return Errors.Login.IncorrectEmailOrPassword;

        if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) is false)
            return Errors.Login.IncorrectEmailOrPassword;

        return CreateToken(user);
    }

    public ErrorOr<Updated> ChangeUserPassword(
        Guid userId,
        string oldPassword,
        string newPassword,
        string confirmNewPassword)
    {
        var user = userRepository.FindUserById(userId);

        if (user is null)
            return Errors.User.NotFound;

        if (VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt) is false)
            return Errors.Login.IncorrectEmailOrPassword;

        if (newPassword != confirmNewPassword)
            return Errors.User.PasswordsDontMatch;

        if (newPassword == oldPassword)
            return Errors.User.NewAndOldPasswordAreTheSame;

        var result = user.ChangePassword(newPassword);

        if (result == Result.Updated)
            userRepository.SaveChanges();

        return result;
    }

    public bool IsUserExists(string email)
    {
        return userRepository.IsUserExists(email);
    }

    private static bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
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
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}