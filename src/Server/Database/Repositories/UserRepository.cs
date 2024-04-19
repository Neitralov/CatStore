namespace Database.Repositories;

public class UserRepository(DatabaseContext database) : IUserRepository
{
    public async Task AddUser(User user)
    {
        await database.AddAsync(user);
    }

    public async Task AddRefreshTokenSession(RefreshTokenSession refreshTokenSession)
    {
        await database.AddAsync(refreshTokenSession);
    }

    public async Task<User?> FindUserById(Guid userId)
    {
        return await database.Users.SingleOrDefaultAsync(user => user.UserId == userId);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await database.Users.SingleOrDefaultAsync(user => user.Email == email);
    }

    public async Task<int> GetNumberOfUsersRefreshTokenSessions(Guid userId)
    {
        return await database.RefreshTokenSessions.CountAsync(refreshTokenSession => refreshTokenSession.UserId == userId);
    }

    public async Task<RefreshTokenSession?> GetUserRefreshTokenSession(Guid userId, string oldRefreshToken)
    {
        return await database.RefreshTokenSessions.SingleOrDefaultAsync(refreshTokenSession =>
            refreshTokenSession.UserId == userId &&
            refreshTokenSession.Token == oldRefreshToken &&
            refreshTokenSession.ExpirationDate >= DateTime.UtcNow);
    }
    
    public async Task RemoveAllUsersRefreshTokenSessions(Guid userId)
    {
        var usersRefreshTokenSessions = await database.RefreshTokenSessions.Where(refreshTokenSession => refreshTokenSession.UserId == userId).ToListAsync();
        database.RefreshTokenSessions.RemoveRange(usersRefreshTokenSessions);
    }

    public async Task<bool> IsUserExists(string email)
    {
        return await database.Users.AnyAsync(user => user.Email == email);
    }

    public async Task SaveChanges()
    {
        database.SavingChanges += async (_, _) => await DeleteAllInvalidRefreshTokenSessions();
        await database.SaveChangesAsync();
    }

    private async Task DeleteAllInvalidRefreshTokenSessions()
    {
        var invalidSessions = await database.RefreshTokenSessions.Where(session => session.ExpirationDate < DateTime.UtcNow).ToListAsync();
        database.RemoveRange(invalidSessions);
    }
}