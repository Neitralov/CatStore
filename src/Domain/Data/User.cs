namespace Domain.Data;

public class User
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public DateTime DateCreated { get; private set; }
    public string Role { get; private set; } = "customer";

    private User() { }

    //AuthService authService,
    public static ErrorOr<User> Create(
        string email,
        string password,
        string confirmPassword,
        Guid? userId = null)
    {
        List<Error> errors = new();

        //if (authService.IsUserExists(email))
        //    errors.Add(Errors.User.AlreadyExists);

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

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}