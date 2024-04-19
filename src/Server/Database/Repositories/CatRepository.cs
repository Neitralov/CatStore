namespace Database.Repositories;

public class CatRepository(DatabaseContext database) : ICatRepository
{
    public async Task AddCat(Cat cat)
    {
        await database.AddAsync(cat);
    }

    public async Task<Cat?> FindCatById(Guid catId)
    {
        return await database.Cats.SingleOrDefaultAsync(cat => cat.CatId == catId);
    }

    public async Task<Cat?> GetCat(Guid catId)
    {
        return await database.Cats
            .AsNoTracking()
            .SingleOrDefaultAsync(cat => cat.CatId == catId);
    }

    public async Task<List<Cat>> GetCats()
    {
        return await database.Cats.AsNoTracking().ToListAsync();
    }

    public async Task<bool> RemoveCat(Guid catId)
    {
        var storedCat = await database.Cats.FindAsync(catId);

        if (storedCat is not null)
            database.Remove(storedCat);

        return storedCat is not null;
    }

    public async Task<bool> IsCatExists(string name)
    {
        return await database.Cats.AnyAsync(cat => cat.Name == name);
    }

    public async Task<bool> IsCatExists(Guid catId)
    {
        return await database.Cats.AnyAsync(cat => cat.CatId == catId);
    }

    public async Task SaveChanges() => await database.SaveChangesAsync();
}