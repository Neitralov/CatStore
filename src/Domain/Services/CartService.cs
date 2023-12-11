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
        var sameCartItem = _repository.GetSameCartItem(cartItem);

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
}