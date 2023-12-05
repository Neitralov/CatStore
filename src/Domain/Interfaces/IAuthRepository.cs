namespace Domain.Interfaces;

public interface IAuthRepository
{
    void AddUser(User user);
    bool IsUserExists(string email);
    void SaveChanges();
}