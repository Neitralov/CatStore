namespace Domain.Interfaces;

public interface ICatRepository
{
    void AddCat(Cat cat);
    Cat? GetCat(Guid catId);
    IEnumerable<Cat> GetAllCats();
    bool UpdateCat(Cat cat);
    bool RemoveCat(Guid catId);
    bool IsCatExists(string name);
    bool IsCatExists(Guid catId);
    void SaveChanges();
}