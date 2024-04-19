namespace Domain.Services;

public class CartService(ICartRepository cartRepository, ICatRepository catRepository)
{
    public async Task<ErrorOr<Created>> StoreCartItem(CartItem cartItem)
    {
        if (await catRepository.IsCatExists(cartItem.CatId) is false)
            return Errors.Cat.NotFound;
        
        var sameCartItem = await cartRepository.FindCartItem(cartItem);

        if (sameCartItem is null)
            await cartRepository.AddCartItem(cartItem);
        else
            sameCartItem.IncreaseQuantity();

        await cartRepository.SaveChanges();

        return Result.Created;
    }

    public async Task<ErrorOr<CartItem>> GetCartItem(Guid userId, Guid catId)
    {
        var result = await cartRepository.GetCartItem(userId, catId);

        return result is not null ? result : Errors.CartItem.NotFound;
    }
    
    public async Task<List<CartItem>> GetCartItems(Guid userId)
    {
        return await cartRepository.GetCartItems(userId);
    }

    public async Task<ErrorOr<Deleted>> DeleteCartItem(Guid userId, Guid catId)
    {
        var result = await cartRepository.RemoveCartItem(userId, catId);
        await cartRepository.SaveChanges();

        return result ? Result.Deleted : Errors.CartItem.NotFound;
    }

    public async Task<ErrorOr<Updated>> UpdateQuantity(Guid userId, Guid catId, int quantity)
    {
        var cartItem = await cartRepository.FindCartItem(userId, catId);

        if (cartItem is null)
            return Errors.CartItem.NotFound;
        
        var result = cartItem.UpdateQuantity(quantity);

        if (result == Result.Updated)
            await cartRepository.SaveChanges();

        return result;
    }

    public async Task<ErrorOr<int>> GetUserCartItemsCount(Guid userId)
    {
        var result = await cartRepository.GetUserCartItemsCount(userId);

        return result;
    }
}