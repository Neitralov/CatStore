namespace Domain.Interfaces;

public interface IUserRepository
{
    //Create
    void AddUser(User user);
    void AddRefreshTokenSession(RefreshTokenSession refreshTokenSession);

    //Read
    User? FindUserById(Guid userId);
    User? FindUserByEmail(string email);
    int GetNumberOfUsersRefreshTokenSessions(Guid userId);
    RefreshTokenSession? GetUserRefreshTokenSession(Guid userId, string refreshToken);
    bool IsUserExists(string email);

    //Update

    //Delete
    void RemoveUser(User user);
    void RemoveAllUsersRefreshTokenSessions(Guid userId);

    //Other
    void SaveChanges();
}