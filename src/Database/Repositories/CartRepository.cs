namespace Database.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DatabaseContext _database;

    public CartRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _database = factory.CreateDbContext();
    }

    public void StoreCartItem(CartItem cartItem)
    {
        _database.Add(cartItem);
    }

    public bool RemoveCartItem(Guid userId, Guid catId)
    {
        var cartItem = _database.CartItems.SingleOrDefault(cartItem =>
            cartItem.UserId == userId &&
            cartItem.CatId == catId);

        if (cartItem is { })
            _database.Remove(cartItem);

        return cartItem is { };
    }

    public bool RemoveCartItems(List<CartItem> items)
    {
        var dbCartItems = new List<CartItem>();

        foreach (var item in items)
        {
            var dbCartItem = _database.CartItems.SingleOrDefault(dbItem =>
                dbItem.UserId == item.UserId &&
                dbItem.CatId == item.CatId);

            if (dbCartItem is { })
                dbCartItems.Add(dbCartItem);
            else
                return false;
        }

        _database.CartItems.RemoveRange(dbCartItems);
        return true;
    }

    public CartItem? FindCartItem(CartItem cartItem)
    {
        return _database.CartItems.SingleOrDefault(item =>
            item.UserId == cartItem.UserId &&
            item.CatId == cartItem.CatId);
    }

    public IEnumerable<CartItem> GetAllUserCartItems(Guid userId)
    {
        return _database.CartItems.Where(cartItem => cartItem.UserId == userId);
    }

    public int GetUserCartItemsCount(Guid userId)
    {
        return _database.CartItems.Count(item => item.UserId == userId);
    }

    public void SaveChanges()
    {
        _database.SaveChanges();
    }
}