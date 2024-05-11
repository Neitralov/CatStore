namespace Database.Repositories;

public class UserRepository(IMongoDatabase database) : IUserRepository
{
    public async Task AddUser(User user)
    {
        var collection = database.GetCollection<User>("users");
        
        await collection.InsertOneAsync(user);
    }

    public async Task AddRefreshTokenSession(RefreshTokenSession refreshTokenSession)
    {
        var collection = database.GetCollection<RefreshTokenSession>("tokenSessions");
        
        await collection.InsertOneAsync(refreshTokenSession);
    }

    public async Task<User?> FindUserById(Guid userId)
    {
        var collection = database.GetCollection<User>("users");
        
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(user => user.UserId, userId);

        return await collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        var collection = database.GetCollection<User>("users");
        
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(user => user.Email, email);
        
        return await collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<int> GetNumberOfUsersRefreshTokenSessions(Guid userId)
    {
        var collection = database.GetCollection<RefreshTokenSession>("tokenSessions");
        
        var builder = Builders<RefreshTokenSession>.Filter;
        var filter = builder.Eq(tokenSession => tokenSession.UserId, userId);
        
        return (int)await collection.CountDocumentsAsync(filter);
    }

    public async Task<RefreshTokenSession?> GetUserRefreshTokenSession(Guid userId, string oldRefreshToken)
    {
        var collection = database.GetCollection<RefreshTokenSession>("tokenSessions");
        
        var builder = Builders<RefreshTokenSession>.Filter;
        var filter = 
            builder.Eq(tokenSession => tokenSession.UserId, userId) & 
            builder.Eq(tokenSession => tokenSession.Token, oldRefreshToken) & 
            builder.Gte(tokenSession => tokenSession.ExpirationDate, DateTime.UtcNow);
        
        return await collection.Find(filter).SingleOrDefaultAsync();
    }
    
    public async Task RemoveAllUsersRefreshTokenSessions(Guid userId)
    {
        var collection = database.GetCollection<RefreshTokenSession>("tokenSessions");
        
        var builder = Builders<RefreshTokenSession>.Filter;
        var filter = builder.Eq(tokenSession => tokenSession.UserId, userId);

        await collection.DeleteManyAsync(filter);
    }

    public async Task<bool> IsUserExists(string email)
    {
        var collection = database.GetCollection<User>("users");
        
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(user => user.Email, email);

        return await collection.Find(filter).AnyAsync();
    }

    public async Task SaveChanges()
    {
        await DeleteAllInvalidRefreshTokenSessions();
    }

    private async Task DeleteAllInvalidRefreshTokenSessions()
    {
        var collection = database.GetCollection<RefreshTokenSession>("tokenSessions");
        
        var builder = Builders<RefreshTokenSession>.Filter;
        var filter = builder.Lt(tokenSession => tokenSession.ExpirationDate, DateTime.UtcNow);

        await collection.DeleteManyAsync(filter);
    }
}