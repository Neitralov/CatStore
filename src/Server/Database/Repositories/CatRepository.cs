namespace Database.Repositories;

public class CatRepository(IDbContextFactory<DatabaseContext> factory) : ICatRepository
{
    private DatabaseContext Database { get; } = factory.CreateDbContext();

    public void AddCat(Cat cat)
    {
        Database.Add(cat);
    }

    public Cat? GetCat(Guid catId)
    {
        return Database.Cats
            .AsNoTracking()
            .SingleOrDefault(cat => cat.CatId == catId);
    }

    public Cat? FindCatById(Guid catId)
    {
        return Database.Cats.SingleOrDefault(cat => cat.CatId == catId);
    }

    public IEnumerable<Cat> GetAllCats()
    {
        return Database.Cats.AsNoTracking();
    }

    public bool RemoveCat(Guid catId)
    {
        var storedCat = Database.Cats.Find(catId);

        if (storedCat is { })
            Database.Remove(storedCat);

        return storedCat is { };
    }

    public bool IsCatExists(string name)
    {
        return Database.Cats.Any(cat => cat.Name == name);
    }

    public bool IsCatExists(Guid catId)
    {
        return Database.Cats.Any(cat => cat.CatId == catId);
    }

    public void SaveChanges() => Database.SaveChanges();
}