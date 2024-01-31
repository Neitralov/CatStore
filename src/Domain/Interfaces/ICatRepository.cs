namespace Domain.Interfaces;

public interface ICatRepository
{
    //Create
    void AddCat(Cat cat);

    //Read
    Cat? GetCat(Guid catId);
    IEnumerable<Cat> GetAllCats();
    bool IsCatExists(string name);
    bool IsCatExists(Guid catId);

    //Update
    bool UpdateCat(Cat cat);

    //Delete
    bool RemoveCat(Guid catId);

    //Other
    void SaveChanges();
}