namespace Database.Repositories;

public class UserRepository(IDbContextFactory<DatabaseContext> factory) : IUserRepository
{
    private DatabaseContext Database { get; } = factory.CreateDbContext();

    public void AddUser(User user)
    {
        Database.Add(user);
    }

    public void AddRefreshTokenSession(RefreshTokenSession refreshTokenSession)
    {
        Database.Add(refreshTokenSession);
    }

    public User? FindUserById(Guid userId)
    {
        return Database.Users.SingleOrDefault(user => user.UserId == userId);
    }

    public User? FindUserByEmail(string email)
    {
        return Database.Users.SingleOrDefault(user => user.Email == email);
    }

    public int GetNumberOfUsersRefreshTokenSessions(Guid userId)
    {
        return Database.RefreshTokenSessions.Count(refreshTokenSession => refreshTokenSession.UserId == userId);
    }

    public RefreshTokenSession? GetUserRefreshTokenSession(Guid userId, string oldRefreshToken)
    {
        return Database.RefreshTokenSessions.SingleOrDefault(refreshTokenSession =>
            refreshTokenSession.UserId == userId &&
            refreshTokenSession.Token == oldRefreshToken &&
            refreshTokenSession.ExpirationDate >= DateTime.UtcNow);
    }

    public void RemoveUser(User user)
    {
        Database.Remove(user);
    }

    public void RemoveAllUsersRefreshTokenSessions(Guid userId)
    {
        var usersRefreshTokenSessions = Database.RefreshTokenSessions.Where(refreshTokenSession => refreshTokenSession.UserId == userId);
        Database.RefreshTokenSessions.RemoveRange(usersRefreshTokenSessions);
    }

    public bool IsUserExists(string email)
    {
        return Database.Users.Any(user => user.Email == email);
    }

    public void SaveChanges()
    {
        Database.SavingChanges += (_, _) => DeleteAllInvalidRefreshTokenSessions();
        Database.SaveChanges();
    }

    private void DeleteAllInvalidRefreshTokenSessions()
    {
        var invalidSessions = Database.RefreshTokenSessions.Where(session => session.ExpirationDate < DateTime.UtcNow);
        Database.RemoveRange(invalidSessions);
    }
}