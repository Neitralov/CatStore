namespace Domain.Services;

public class CartService
{
    private readonly ICartRepository _repository;
    private readonly ICatRepository _catRepository;
   
    public CartService(ICartRepository repository, ICatRepository catRepository)
    {
        _repository = repository;
        _catRepository = catRepository;
    }

    public ErrorOr<Created> StoreCartItem(CartItem cartItem)
    {
        var sameCartItem = _repository.FindCartItem(cartItem);

        if (sameCartItem is null)
        {
            if (_catRepository.IsCatExists(cartItem.CatId) is false)
                return Errors.Cat.NotFound;

            _repository.StoreCartItem(cartItem);
        }
        else
            sameCartItem.IncreaseQuantity();

        _repository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<IEnumerable<CartItem>> GetAllUserCartItems(Guid userId)
    {
        return _repository.GetAllUserCartItems(userId).ToList();
    }

    public ErrorOr<Deleted> RemoveCartItem(Guid userId, Guid catId)
    {
        var result = _repository.RemoveCartItem(userId, catId);
        _repository.SaveChanges();

        return result ? Result.Deleted : Errors.CartItem.NotFound;
    }

    public ErrorOr<Updated> UpdateQuantity(CartItem cartItem)
    {
        var dbCartItem = _repository.FindCartItem(cartItem);

        if (dbCartItem is null)
            return Errors.CartItem.NotFound;

        var result = dbCartItem.UpdateQuantity(cartItem.Quantity);

        if (result == Result.Updated)
            _repository.SaveChanges();

        return result;
    }

    public ErrorOr<int> GetUserCartItemsCount(Guid userId)
    {
        var result = _repository.GetUserCartItemsCount(userId);

        return result;
    }
}