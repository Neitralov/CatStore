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

    public CartItem? GetCartItem(Guid userId, Guid catId)
    {
        return Database.CartItems
            .AsNoTracking()
            .SingleOrDefault(cartItem =>
                cartItem.UserId == userId &&
                cartItem.CatId == catId);
    }

    public IEnumerable<CartItem> GetCartItems(Guid userId)
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

        if (cartItem is not null)
            Database.Remove(cartItem);

        return cartItem is not null;
    }

    public bool RemoveCartItems(List<CartItem> items)
    {
        var dbCartItems = new List<CartItem>();

        foreach (var item in items)
        {
            var dbCartItem = Database.CartItems.SingleOrDefault(dbItem =>
                dbItem.UserId == item.UserId &&
                dbItem.CatId == item.CatId);

            if (dbCartItem is not null)
                dbCartItems.Add(dbCartItem);
            else
                return false;
        }

        Database.CartItems.RemoveRange(dbCartItems);
        return true;
    }

    public void SaveChanges() => Database.SaveChanges();
}