namespace Domain.Interfaces;

public interface ICatRepository
{
    void AddCat(Cat cat);
    Cat? GetCat(Guid catId);
    bool UpdateCat(Cat cat);
    bool DeleteCat(Guid catId);
    IEnumerable<Cat> GetAllCats();
    bool IsCatExists(string name);
    void SaveChanges();
}