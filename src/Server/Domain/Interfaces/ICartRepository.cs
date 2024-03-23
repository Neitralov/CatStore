namespace Domain.Interfaces;

public interface ICartRepository
{
    //Create
    void AddCartItem(CartItem cartItem);

    //Read
    CartItem? FindCartItem(CartItem cartItem);
    CartItem? GetCartItem(Guid userId, Guid catId);
    IEnumerable<CartItem> GetCartItems(Guid userId);
    int GetUserCartItemsCount(Guid userId);

    //Update

    //Delete
    bool RemoveCartItem(Guid userId, Guid catId);
    bool RemoveCartItems(List<CartItem> items);

    //Other
    void SaveChanges();
}