namespace Domain.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICatRepository _catRepository;
   
    public CartService(ICartRepository cartRepository, ICatRepository catRepository)
    {
        _cartRepository = cartRepository;
        _catRepository = catRepository;
    }

    public ErrorOr<Created> StoreCartItem(CartItem cartItem)
    {
        var sameCartItem = _cartRepository.FindCartItem(cartItem);

        if (sameCartItem is null)
        {
            if (_catRepository.IsCatExists(cartItem.CatId) is false)
                return Errors.Cat.NotFound;

            _cartRepository.StoreCartItem(cartItem);
        }
        else
            sameCartItem.IncreaseQuantity();

        _cartRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<IEnumerable<CartItem>> GetAllUserCartItems(Guid userId)
    {
        return _cartRepository.GetAllUserCartItems(userId).ToList();
    }

    public ErrorOr<Deleted> RemoveCartItem(Guid userId, Guid catId)
    {
        var result = _cartRepository.RemoveCartItem(userId, catId);
        _cartRepository.SaveChanges();

        return result ? Result.Deleted : Errors.CartItem.NotFound;
    }

    public ErrorOr<Updated> UpdateQuantity(CartItem cartItem)
    {
        var dbCartItem = _cartRepository.FindCartItem(cartItem);

        if (dbCartItem is null)
            return Errors.CartItem.NotFound;

        var result = dbCartItem.UpdateQuantity(cartItem.Quantity);

        if (result == Result.Updated)
            _cartRepository.SaveChanges();

        return result;
    }

    public ErrorOr<int> GetUserCartItemsCount(Guid userId)
    {
        var result = _cartRepository.GetUserCartItemsCount(userId);

        return result;
    }
}