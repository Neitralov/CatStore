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

        return result is { } ? result : Errors.CartItem.NotFound;
    }

    public ErrorOr<IEnumerable<CartItem>> GetCartItems(Guid userId)
    {
        return cartRepository.GetCartItems(userId).ToList();
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

        //TODO: А почему я создаю новый cartItem и извлекаю его количество, а не передаю CatId и UserId, а кол-во отдельно?
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