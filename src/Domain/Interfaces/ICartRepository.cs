namespace Domain.Interfaces;

public interface ICartRepository
{
    void StoreCartItem(CartItem cartItem);
    bool RemoveCartItem(Guid userId, Guid catId);
    CartItem? FindCartItem(CartItem cartItem);
    IEnumerable<CartItem> GetAllUserCartItems(Guid userId);
    int GetUserCartItemsCount(Guid userId);
    void SaveChanges();
}