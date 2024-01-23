namespace Domain.Services;

public class CartService(ICartRepository cartRepository, ICatRepository catRepository)
{
    public ErrorOr<Created> StoreCartItem(CartItem cartItem)
    {
        var sameCartItem = cartRepository.FindCartItem(cartItem);

        if (sameCartItem is null)
        {
            if (catRepository.IsCatExists(cartItem.CatId) is false)
                return Errors.Cat.NotFound;

            cartRepository.AddCartItem(cartItem);
        }
        else
            sameCartItem.IncreaseQuantity();

        cartRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<IEnumerable<CartItem>> GetAllUserCartItems(Guid userId)
    {
        return cartRepository.GetAllUserCartItems(userId).ToList();
    }

    public ErrorOr<Deleted> DeleteCartItem(Guid userId, Guid catId)
    {
        var result = cartRepository.RemoveCartItem(userId, catId);
        cartRepository.SaveChanges();

        return result ? Result.Deleted : Errors.CartItem.NotFound;
    }

    public ErrorOr<Updated> UpdateQuantity(CartItem cartItem)
    {
        var dbCartItem = cartRepository.FindCartItem(cartItem);

        if (dbCartItem is null)
            return Errors.CartItem.NotFound;

        var result = dbCartItem.UpdateQuantity(cartItem.Quantity);

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