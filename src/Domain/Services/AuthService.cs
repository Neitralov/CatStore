namespace Domain.Services;

public class AuthService
{
    private readonly IAuthRepository _repository;

    public AuthService(IAuthRepository repository)
    {
        _repository = repository;
    }

    public ErrorOr<Created> StoreUser(User user)
    {
        if (IsUserExists(user.Email))
            return Errors.User.AlreadyExists;

        _repository.AddUser(user);
        _repository.SaveChanges();

        return Result.Created;
    }

    public bool IsUserExists(string email)
    {
        return _repository.IsUserExists(email);
    }
}