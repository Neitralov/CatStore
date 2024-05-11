namespace Database.Repositories;

public class CatRepository(IMongoDatabase database) : ICatRepository
{
    public async Task AddCat(Cat cat)
    {
        var collection = database.GetCollection<Cat>("cats");
        
        await collection.InsertOneAsync(cat);
    }

    public async Task<Cat?> FindCatById(Guid catId)
    {
        var collection = database.GetCollection<Cat>("cats");
        
        var builder = Builders<Cat>.Filter;
        var filter = builder.Eq(cat => cat.CatId, catId);

        return await collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<Cat?> GetCat(Guid catId)
    {
        return await FindCatById(catId);
    }

    public async Task<List<Cat>> GetCats()
    {
        var collection = database.GetCollection<Cat>("cats");

        return await collection.Find("{ }").ToListAsync();
    }
    
    public async Task ReplaceCat(Cat cat)
    {
        var collection = database.GetCollection<Cat>("cats");
        
        var builder = Builders<Cat>.Filter;
        var filter = builder.Eq(field => field.CatId, cat.CatId);

        await collection.ReplaceOneAsync(filter, cat);
    }

    public async Task<bool> RemoveCat(Guid catId)
    {
        var collection = database.GetCollection<Cat>("cats");
        
        var builder = Builders<Cat>.Filter;
        var filter = builder.Eq(cat => cat.CatId, catId);
        
        var result = await collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<bool> IsCatExists(string name)
    {
        var collection = database.GetCollection<Cat>("cats");
        
        var builder = Builders<Cat>.Filter;
        var filter = builder.Eq(cat => cat.Name, name);

        return await collection.Find(filter).AnyAsync();
    }

    public async Task<bool> IsCatExists(Guid catId)
    {
        var collection = database.GetCollection<Cat>("cats");
        
        var builder = Builders<Cat>.Filter;
        var filter = builder.Eq(cat => cat.CatId, catId);
        
        return await collection.Find(filter).AnyAsync();
    }

    public async Task SaveChanges() => await Task.CompletedTask;
}