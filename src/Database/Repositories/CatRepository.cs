namespace Database.Repositories;

public class CatRepository : ICatRepository
{
    private readonly DatabaseContext _database;

    public CatRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _database = factory.CreateDbContext();
    }

    public void AddCat(Cat cat)
    {
        _database.Add(cat);
    }

    public Cat? GetCat(Guid catId)
    {
        return _database.Cats
            .AsNoTracking()
            .SingleOrDefault(cat => cat.CatId == catId);
    }

    public IEnumerable<Cat> GetAllCats()
    {
        return _database.Cats.AsNoTracking();
    }

    public bool UpdateCat(Cat cat)
    {
        var storedCat = _database.Cats.Find(cat.CatId);

        if (storedCat is { })
            storedCat.UpdateCat(cat);

        return storedCat is { };
    }

    public bool RemoveCat(Guid catId)
    {
        var storedCat = _database.Cats.Find(catId);

        if (storedCat is { })
            _database.Remove(storedCat);

        return storedCat is { };
    }

    public bool IsCatExists(string name)
    {
        return _database.Cats.Any(cat => cat.Name == name);
    }

    public bool IsCatExists(Guid catId)
    {
        return _database.Cats.Any(cat => cat.CatId == catId);
    }

    public void SaveChanges() => _database.SaveChanges();
}