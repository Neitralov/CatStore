namespace Domain.Interfaces;

public interface ICartRepository
{
    //Create
    Task AddCartItem(CartItem cartItem);

    //Read
    Task<CartItem?> FindCartItem(Guid userId, Guid catId);
    Task<CartItem?> FindCartItem(CartItem cartItem);
    Task<CartItem?> GetCartItem(Guid userId, Guid catId);
    Task<List<CartItem>> GetCartItems(Guid userId);
    Task<int> GetUserCartItemsCount(Guid userId);

    //Update

    //Delete
    Task<bool> RemoveCartItem(Guid userId, Guid catId);
    Task<bool> RemoveCartItems(List<CartItem> items);

    //Other
    Task SaveChanges();
}