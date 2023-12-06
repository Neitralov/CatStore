namespace Domain.Interfaces;

public interface IAuthRepository
{
    void AddUser(User user);
    User? FindUserByEmail(string email);
    bool IsUserExists(string email);
    void SaveChanges();
}