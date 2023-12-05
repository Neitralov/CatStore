namespace WebAPI.Contracts.User;

public record CreateUserRequest(
    string email,
    string password,
    string confirmPassword);