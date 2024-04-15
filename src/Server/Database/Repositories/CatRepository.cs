namespace Database.Repositories;

public class CatRepository(DatabaseContext database) : ICatRepository
{
    public void AddCat(Cat cat)
    {
        database.Add(cat);
    }

    public Cat? FindCatById(Guid catId)
    {
        return database.Cats.SingleOrDefault(cat => cat.CatId == catId);
    }

    public Cat? GetCat(Guid catId)
    {
        return database.Cats
            .AsNoTracking()
            .SingleOrDefault(cat => cat.CatId == catId);
    }

    public List<Cat> GetCats()
    {
        return database.Cats.AsNoTracking().ToList();
    }

    public bool RemoveCat(Guid catId)
    {
        var storedCat = database.Cats.Find(catId);

        if (storedCat is not null)
            database.Remove(storedCat);

        return storedCat is not null;
    }

    public bool IsCatExists(string name)
    {
        return database.Cats.Any(cat => cat.Name == name);
    }

    public bool IsCatExists(Guid catId)
    {
        return database.Cats.Any(cat => cat.CatId == catId);
    }

    public void SaveChanges() => database.SaveChanges();
}