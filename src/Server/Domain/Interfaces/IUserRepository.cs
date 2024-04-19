namespace Domain.Interfaces;

public interface IUserRepository
{
    //Create
    Task AddUser(User user);
    Task AddRefreshTokenSession(RefreshTokenSession refreshTokenSession);

    //Read
    Task<User?> FindUserById(Guid userId);
    Task<User?> FindUserByEmail(string email);
    Task<int> GetNumberOfUsersRefreshTokenSessions(Guid userId);
    Task<RefreshTokenSession?> GetUserRefreshTokenSession(Guid userId, string refreshToken);
    Task<bool> IsUserExists(string email);

    //Update

    //Delete
    Task RemoveAllUsersRefreshTokenSessions(Guid userId);

    //Other
    Task SaveChanges();
}