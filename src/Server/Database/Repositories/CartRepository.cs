namespace Database.Repositories;

public class CartRepository(DatabaseContext database) : ICartRepository
{
    public void AddCartItem(CartItem cartItem)
    {
        database.Add(cartItem);
    }

    public CartItem? FindCartItem(CartItem cartItem)
    {
        return database.CartItems.SingleOrDefault(item =>
            item.UserId == cartItem.UserId &&
            item.CatId == cartItem.CatId);
    }

    public CartItem? GetCartItem(Guid userId, Guid catId)
    {
        return database.CartItems
            .AsNoTracking()
            .SingleOrDefault(cartItem =>
                cartItem.UserId == userId &&
                cartItem.CatId == catId);
    }

    public IEnumerable<CartItem> GetCartItems(Guid userId)
    {
        return database.CartItems
            .AsNoTracking()
            .Where(cartItem => cartItem.UserId == userId);
    }

    public int GetUserCartItemsCount(Guid userId)
    {
        return database.CartItems
            .Where(item => item.UserId == userId)
            .Sum(item => item.Quantity);
    }

    public bool RemoveCartItem(Guid userId, Guid catId)
    {
        var cartItem = database.CartItems.SingleOrDefault(cartItem =>
            cartItem.UserId == userId &&
            cartItem.CatId == catId);

        if (cartItem is not null)
            database.Remove(cartItem);

        return cartItem is not null;
    }

    public bool RemoveCartItems(List<CartItem> items)
    {
        var dbCartItems = new List<CartItem>();

        foreach (var item in items)
        {
            var dbCartItem = database.CartItems.SingleOrDefault(dbItem =>
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

    public void SaveChanges() => database.SaveChanges();
}