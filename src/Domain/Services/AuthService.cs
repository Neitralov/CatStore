namespace Domain.Services;

public class AuthService
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(IAuthRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public ErrorOr<Created> StoreUser(User user)
    {
        if (IsUserExists(user.Email))
            return Errors.User.AlreadyExists;

        _repository.AddUser(user);
        _repository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<string> Login(string email, string password)
    {
        var user = _repository.FindUserByEmail(email);

        if (user is null)
            return "Error";
            // Вернуть ошибку NotFound

        if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) is false)
            return "Error";
            // Вернуть ошибку PasswordIncorrect

        return CreateToken(user);
    }

    public bool IsUserExists(string email)
    {
        return _repository.IsUserExists(email);
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
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