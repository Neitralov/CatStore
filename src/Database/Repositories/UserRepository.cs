namespace Database.Repositories;

public class UserRepository(IDbContextFactory<DatabaseContext> factory) : IUserRepository
{
    private DatabaseContext Database { get; } = factory.CreateDbContext();

    public void AddUser(User user)
    {
        Database.Add(user);
    }

    public User? FindUserById(Guid userId)
    {
        return Database.Users.SingleOrDefault(user => user.UserId == userId);
    }

    public User? FindUserByEmail(string email)
    {
        return Database.Users.SingleOrDefault(user => user.Email == email);
    }

    public void RemoveUser(User user)
    {
        Database.Remove(user);
    }

    public bool IsUserExists(string email)
    {
        return Database.Users.Any(user => user.Email == email);
    }

    public void SaveChanges() => Database.SaveChanges();
}