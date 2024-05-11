namespace Database.Repositories;

public class CartRepository(IMongoDatabase database) : ICartRepository
{
    public async Task AddCartItem(CartItem cartItem)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        await collection.InsertOneAsync(cartItem);
    }

    public async Task<CartItem?> FindCartItem(Guid userId, Guid catId)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        var builder = Builders<CartItem>.Filter;
        var filter = builder.Eq(item => item.UserId, userId) & builder.Eq(item => item.CatId, catId);
        
        return await collection.Find(filter).SingleOrDefaultAsync();
    }
    
    public async Task<CartItem?> FindCartItem(CartItem cartItem)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        var builder = Builders<CartItem>.Filter;
        var filter = builder.Eq(item => item.UserId, cartItem.UserId) & builder.Eq(item => item.CatId, cartItem.CatId);
        
        return await collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<CartItem?> GetCartItem(Guid userId, Guid catId)
    {
        return await FindCartItem(userId, catId);
    }

    public async Task<List<CartItem>> GetCartItems(Guid userId)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        var builder = Builders<CartItem>.Filter;
        var filter = builder.Eq(item => item.UserId, userId);
        
        return await collection.Find(filter).ToListAsync();
    }

    public async Task<int> GetUserCartItemsCount(Guid userId)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        return await Task.FromResult(collection.AsQueryable().Where(item => item.UserId == userId).Sum(item => item.Quantity));
    }
    
    public async Task ReplaceCartItem(CartItem cartItem)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        var builder = Builders<CartItem>.Filter;
        var filter = builder.Eq(item => item.UserId, cartItem.UserId) & builder.Eq(item => item.CatId, cartItem.CatId);

        await collection.ReplaceOneAsync(filter, cartItem);
    }

    public async Task<bool> RemoveCartItem(Guid userId, Guid catId)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        var builder = Builders<CartItem>.Filter;
        var filter = builder.Eq(item => item.UserId, userId) & builder.Eq(item => item.CatId, catId);
        
        var result = await collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<bool> RemoveCartItems(List<CartItem> items)
    {
        var collection = database.GetCollection<CartItem>("cartItems");
        
        foreach (var item in items)
        {
            var builder = Builders<CartItem>.Filter;
            var filter = builder.Eq(field => field.UserId, item.UserId) & builder.Eq(field => field.CatId, item.CatId);

            await collection.DeleteOneAsync(filter);
        }
        
        return true;
    }

    public async Task SaveChanges() => await Task.CompletedTask;
}