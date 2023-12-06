namespace WebAPI.Contracts.User;

public record ChangeUserPasswordRequest(
    string OldPassword,
    string NewPassword,
    string ConfirmNewPassword);