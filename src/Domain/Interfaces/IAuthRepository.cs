namespace Domain.Interfaces;

public interface IAuthRepository
{
    void AddUser(User user);
    void RemoveUser(User user);
    User? FindUserByEmail(string email);
    User? FindUserById(Guid userId);
    bool IsUserExists(string email);
    void SaveChanges();
}