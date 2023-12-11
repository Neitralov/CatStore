namespace Database;

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

    public CartItem? GetSameCartItem(CartItem cartItem)
    {
        return _database.CartItems.SingleOrDefault(item =>
            item.UserId == cartItem.UserId &&
            item.CatId == cartItem.CatId);
    }

    public IEnumerable<CartItem> GetAllUserCartItems(Guid userId)
    {
        return _database.CartItems.Where(cartItem => cartItem.UserId == userId);
    }

    public void SaveChanges()
    {
        _database.SaveChanges();
    }
}