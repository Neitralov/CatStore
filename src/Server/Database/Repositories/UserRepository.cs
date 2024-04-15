namespace Database.Repositories;

public class UserRepository(DatabaseContext database) : IUserRepository
{
    public void AddUser(User user)
    {
        database.Add(user);
    }

    public void AddRefreshTokenSession(RefreshTokenSession refreshTokenSession)
    {
        database.Add(refreshTokenSession);
    }

    public User? FindUserById(Guid userId)
    {
        return database.Users.SingleOrDefault(user => user.UserId == userId);
    }

    public User? FindUserByEmail(string email)
    {
        return database.Users.SingleOrDefault(user => user.Email == email);
    }

    public int GetNumberOfUsersRefreshTokenSessions(Guid userId)
    {
        return database.RefreshTokenSessions.Count(refreshTokenSession => refreshTokenSession.UserId == userId);
    }

    public RefreshTokenSession? GetUserRefreshTokenSession(Guid userId, string oldRefreshToken)
    {
        return database.RefreshTokenSessions.SingleOrDefault(refreshTokenSession =>
            refreshTokenSession.UserId == userId &&
            refreshTokenSession.Token == oldRefreshToken &&
            refreshTokenSession.ExpirationDate >= DateTime.UtcNow);
    }
    
    public void RemoveAllUsersRefreshTokenSessions(Guid userId)
    {
        var usersRefreshTokenSessions = database.RefreshTokenSessions.Where(refreshTokenSession => refreshTokenSession.UserId == userId);
        database.RefreshTokenSessions.RemoveRange(usersRefreshTokenSessions);
    }

    public bool IsUserExists(string email)
    {
        return database.Users.Any(user => user.Email == email);
    }

    public void SaveChanges()
    {
        database.SavingChanges += (_, _) => DeleteAllInvalidRefreshTokenSessions();
        database.SaveChanges();
    }

    private void DeleteAllInvalidRefreshTokenSessions()
    {
        var invalidSessions = database.RefreshTokenSessions.Where(session => session.ExpirationDate < DateTime.UtcNow);
        database.RemoveRange(invalidSessions);
    }
}