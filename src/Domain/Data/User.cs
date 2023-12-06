namespace Domain.Data;

public class User
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public DateTime DateCreated { get; private set; }
    public string Role { get; private set; } = "customer";

    public const int MinPasswordLength = 4;

    private User() { }

    public static ErrorOr<User> Create(
        string email,
        string password,
        string confirmPassword,
        Guid? userId = null)
    {
        List<Error> errors = new();

        if (email.Contains('@') is false)
            errors.Add(Errors.User.InvalidEmail);

        if (password.Length < MinPasswordLength)
            errors.Add(Errors.User.InvalidPassword);

        if (password != confirmPassword)
            errors.Add(Errors.User.PasswordsDontMatch);

        if (errors.Count > 0)
            return errors;

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        return new User
        {
            UserId = userId ?? Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            DateCreated = DateTime.UtcNow
        };
    }

    public ErrorOr<Success> ChangePassword(string newPassword)
    {
        if (newPassword.Length < MinPasswordLength)
            return Errors.User.InvalidPassword;

        CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;

        return Result.Success;
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}