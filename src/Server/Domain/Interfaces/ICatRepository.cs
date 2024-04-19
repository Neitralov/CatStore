namespace Domain.Interfaces;

public interface ICatRepository
{
    //Create
    Task AddCat(Cat cat);

    //Read
    Task<Cat?> FindCatById(Guid catId);
    Task<Cat?> GetCat(Guid catId);
    Task<List<Cat>> GetCats();
    Task<bool> IsCatExists(string name);
    Task<bool> IsCatExists(Guid catId);

    //Delete
    Task<bool> RemoveCat(Guid catId);

    //Other
    Task SaveChanges();
}