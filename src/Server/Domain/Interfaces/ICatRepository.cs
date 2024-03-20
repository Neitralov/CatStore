namespace Domain.Interfaces;

public interface ICatRepository
{
    //Create
    void AddCat(Cat cat);

    //Read
    Cat? FindCatById(Guid catId);
    Cat? GetCat(Guid catId);
    IEnumerable<Cat> GetCats();
    bool IsCatExists(string name);
    bool IsCatExists(Guid catId);

    //Delete
    bool RemoveCat(Guid catId);

    //Other
    void SaveChanges();
}