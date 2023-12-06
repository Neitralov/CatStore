namespace WebAPI.Contracts.User;

public record CreateUserRequest(
    string Email,
    string Password,
    string ConfirmPassword);