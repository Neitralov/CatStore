namespace WebAPI.Contracts.User;

public record ChangeUserPasswordRequest(
    string OldPassword,
    string NewPassword,
    string ConfirmNewPassword)
{
    /// <example>1234</example>
    public string OldPassword { get; init; } = OldPassword;
    /// <example>qwerty</example>
    public string NewPassword { get; init; } = NewPassword;
    /// <example>qwerty</example>
    public string ConfirmNewPassword { get; init; } = ConfirmNewPassword;
}