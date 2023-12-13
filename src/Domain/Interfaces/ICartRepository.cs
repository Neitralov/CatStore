namespace Domain.Interfaces;

public interface ICartRepository
{
    void AddCartItem(CartItem cartItem);
    CartItem? FindCartItem(CartItem cartItem);
    IEnumerable<CartItem> GetAllUserCartItems(Guid userId);
    int GetUserCartItemsCount(Guid userId);
    bool RemoveCartItem(Guid userId, Guid catId);
    bool RemoveCartItems(List<CartItem> items);
    void SaveChanges();
}