namespace WebAPI.Contracts.User;

public record LoginUserRequest(
    string Email,
    string Password)
{
    /// <example>admin@gmail.com</example>
    public string Email { get; init; } = Email;
    /// <example>admin</example>
    public string Password { get; init; } = Password;
}