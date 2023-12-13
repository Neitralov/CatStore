namespace Database;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _database;

    public UserRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _database = factory.CreateDbContext();
    }

    public void AddUser(User user)
    {
        _database.Add(user);
    }

    public void RemoveUser(User user)
    {
        _database.Remove(user);
    }

    public User? FindUserByEmail(string email)
    {
        return _database.Users.SingleOrDefault(user => user.Email == email);
    }

    public User? FindUserById(Guid userId)
    {
        return _database.Users.SingleOrDefault(user => user.UserId == userId);
    }

    public bool IsUserExists(string email)
    {
        return _database.Users.Any(user => user.Email == email);
    }

    public void SaveChanges() => _database.SaveChanges();
}