namespace Domain.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public ErrorOr<Created> StoreUser(User user)
    {
        if (IsUserExists(user.Email))
            return Errors.User.AlreadyExists;

        _userRepository.AddUser(user);
        _userRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteUserById(Guid userId)
    {
        var user = _userRepository.FindUserById(userId);

        if (user is null)
            return Errors.User.NotFound;

        _userRepository.RemoveUser(user);
        _userRepository.SaveChanges();

        return Result.Deleted;
    }

    public ErrorOr<string> Login(string email, string password)
    {
        var user = _userRepository.FindUserByEmail(email);

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
        var user = _userRepository.FindUserById(userId);

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
            _userRepository.SaveChanges();

        return result;
    }

    public bool IsUserExists(string email)
    {
        return _userRepository.IsUserExists(email);
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
        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new (ClaimTypes.Name, user.Email),
            new (ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}