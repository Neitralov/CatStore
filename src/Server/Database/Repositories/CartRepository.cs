namespace Database.Repositories;

public class CartRepository(DatabaseContext database) : ICartRepository
{
    public async Task AddCartItem(CartItem cartItem)
    {
        await database.AddAsync(cartItem);
    }

    public async Task<CartItem?> FindCartItem(Guid userId, Guid catId)
    {
        return await database.CartItems.SingleOrDefaultAsync(item =>
            item.UserId == userId &&
            item.CatId == catId);
    }
    
    public async Task<CartItem?> FindCartItem(CartItem cartItem)
    {
        return await database.CartItems.SingleOrDefaultAsync(item =>
            item.UserId == cartItem.UserId &&
            item.CatId == cartItem.CatId);
    }

    public async Task<CartItem?> GetCartItem(Guid userId, Guid catId)
    {
        return await database.CartItems
            .AsNoTracking()
            .SingleOrDefaultAsync(cartItem =>
                cartItem.UserId == userId &&
                cartItem.CatId == catId);
    }

    public async Task<List<CartItem>> GetCartItems(Guid userId)
    {
        return await database.CartItems
            .AsNoTracking()
            .Where(cartItem => cartItem.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> GetUserCartItemsCount(Guid userId)
    {
        return await database.CartItems
            .Where(item => item.UserId == userId)
            .SumAsync(item => item.Quantity);
    }

    public async Task<bool> RemoveCartItem(Guid userId, Guid catId)
    {
        var cartItem = await database.CartItems.SingleOrDefaultAsync(cartItem =>
            cartItem.UserId == userId &&
            cartItem.CatId == catId);

        if (cartItem is not null)
            database.Remove(cartItem);

        return cartItem is not null;
    }

    public async Task<bool> RemoveCartItems(List<CartItem> items)
    {
        var dbCartItems = new List<CartItem>();

        foreach (var item in items)
        {
            var dbCartItem = await database.CartItems.SingleOrDefaultAsync(dbItem =>
                dbItem.UserId == item.UserId &&
                dbItem.CatId == item.CatId);

            if (dbCartItem is not null)
                dbCartItems.Add(dbCartItem);
            else
                return false;
        }

        database.CartItems.RemoveRange(dbCartItems);
        return true;
    }

    public async Task SaveChanges() => await database.SaveChangesAsync();
}