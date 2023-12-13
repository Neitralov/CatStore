namespace Domain.Interfaces;

public interface IUserRepository
{
    void AddUser(User user);
    User? FindUserById(Guid userId);
    User? FindUserByEmail(string email);
    void RemoveUser(User user);
    bool IsUserExists(string email);
    void SaveChanges();
}