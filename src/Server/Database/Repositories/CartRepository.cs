namespace Database.Repositories;

public class CartRepository(IDbContextFactory<DatabaseContext> factory) : ICartRepository
{
    private DatabaseContext Database { get; } = factory.CreateDbContext();

    public void AddCartItem(CartItem cartItem)
    {
        Database.Add(cartItem);
    }

    public CartItem? FindCartItem(CartItem cartItem)
    {
        return Database.CartItems.SingleOrDefault(item =>
            item.UserId == cartItem.UserId &&
            item.CatId == cartItem.CatId);
    }

    public IEnumerable<CartItem> GetAllUserCartItems(Guid userId)
    {
        return Database.CartItems
            .AsNoTracking()
            .Where(cartItem => cartItem.UserId == userId);
    }

    public int GetUserCartItemsCount(Guid userId)
    {
        return Database.CartItems
            .Where(item => item.UserId == userId)
            .Sum(item => item.Quantity);
    }

    public bool RemoveCartItem(Guid userId, Guid catId)
    {
        var cartItem = Database.CartItems.SingleOrDefault(cartItem =>
            cartItem.UserId == userId &&
            cartItem.CatId == catId);

        if (cartItem is { })
            Database.Remove(cartItem);

        return cartItem is { };
    }

    public bool RemoveCartItems(List<CartItem> items)
    {
        var dbCartItems = new List<CartItem>();

        foreach (var item in items)
        {
            var dbCartItem = Database.CartItems.SingleOrDefault(dbItem =>
                dbItem.UserId == item.UserId &&
                dbItem.CatId == item.CatId);

            if (dbCartItem is { })
                dbCartItems.Add(dbCartItem);
            else
                return false;
        }

        Database.CartItems.RemoveRange(dbCartItems);
        return true;
    }

    public void SaveChanges() => Database.SaveChanges();
}