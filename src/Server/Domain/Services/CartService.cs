namespace Domain.Services;

public class CartService(ICartRepository cartRepository, ICatRepository catRepository)
{
    public ErrorOr<Created> StoreCartItem(CartItem cartItem)
    {
        if (catRepository.IsCatExists(cartItem.CatId) is false)
            return Errors.Cat.NotFound;
        
        var sameCartItem = cartRepository.FindCartItem(cartItem);

        if (sameCartItem is null)
            cartRepository.AddCartItem(cartItem);
        else
            sameCartItem.IncreaseQuantity();

        cartRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<CartItem> GetCartItem(Guid userId, Guid catId)
    {
        var result = cartRepository.GetCartItem(userId, catId);

        return result is not null ? result : Errors.CartItem.NotFound;
    }
    
    public List<CartItem> GetCartItems(Guid userId)
    {
        return cartRepository.GetCartItems(userId);
    }

    public ErrorOr<Deleted> DeleteCartItem(Guid userId, Guid catId)
    {
        var result = cartRepository.RemoveCartItem(userId, catId);
        cartRepository.SaveChanges();

        return result ? Result.Deleted : Errors.CartItem.NotFound;
    }

    public ErrorOr<Updated> UpdateQuantity(Guid userId, Guid catId, int quantity)
    {
        var cartItem = cartRepository.FindCartItem(userId, catId);

        if (cartItem is null)
            return Errors.CartItem.NotFound;
        
        var result = cartItem.UpdateQuantity(quantity);

        if (result == Result.Updated)
            cartRepository.SaveChanges();

        return result;
    }

    public ErrorOr<int> GetUserCartItemsCount(Guid userId)
    {
        var result = cartRepository.GetUserCartItemsCount(userId);

        return result;
    }
}