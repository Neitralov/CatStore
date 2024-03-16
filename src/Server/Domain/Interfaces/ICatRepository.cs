namespace Domain.Interfaces;

public interface ICatRepository
{
    //Create
    void AddCat(Cat cat);

    //Read
    Cat? GetCat(Guid catId);
    Cat? FindCatById(Guid catId);
    IEnumerable<Cat> GetAllCats();
    bool IsCatExists(string name);
    bool IsCatExists(Guid catId);

    //Delete
    bool RemoveCat(Guid catId);

    //Other
    void SaveChanges();
}