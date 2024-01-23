namespace WebAPI.Contracts.User;

public record CreateUserRequest(
    string Email,
    string Password,
    string ConfirmPassword)
{
    /// <example>example@gmail.com</example>
    public string Email { get; init; } = Email;
    /// <example>1234</example>
    public string Password { get; init; } = Password;
    /// <example>1234</example>
    public string ConfirmPassword { get; init; } = ConfirmPassword;
}