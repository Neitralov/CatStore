namespace Domain.Interfaces;

public interface ICartRepository
{
    void StoreCartItem(CartItem cartItem);
    CartItem? GetSameCartItem(CartItem cartItem);
    IEnumerable<CartItem> GetAllUserCartItems(Guid userId);
    void SaveChanges();
}