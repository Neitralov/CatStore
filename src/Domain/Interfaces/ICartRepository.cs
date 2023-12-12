namespace Domain.Interfaces;

public interface ICartRepository
{
    void StoreCartItem(CartItem cartItem);
    bool RemoveCartItem(Guid userId, Guid catId);
    bool RemoveCartItems(List<CartItem> items);
    CartItem? FindCartItem(CartItem cartItem);
    IEnumerable<CartItem> GetAllUserCartItems(Guid userId);
    int GetUserCartItemsCount(Guid userId);
    void SaveChanges();
}