namespace Domain.Interfaces;

public interface ICartRepository
{
    void StoreCartItem(CartItem cartItem);
    bool RemoveCartItem(Guid userId, Guid catId);
    CartItem? GetSameCartItem(CartItem cartItem);
    IEnumerable<CartItem> GetAllUserCartItems(Guid userId);
    void SaveChanges();
}