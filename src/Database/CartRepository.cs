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