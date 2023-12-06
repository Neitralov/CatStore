namespace Database;

public class AuthRepository : IAuthRepository
{
    private readonly DatabaseContext _database;

    public AuthRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _database = factory.CreateDbContext();
    }

    public void AddUser(User user)
    {
        _database.Add(user);
    }

    public User? FindUserByEmail(string email)
    {
        return _database.Users.FirstOrDefault(user => user.Email == email);
    }

    public bool IsUserExists(string email)
    {
        return _database.Users.Any(user => user.Email == email);
    }

    public void SaveChanges() => _database.SaveChanges();
}