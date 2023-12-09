namespace Domain.Services;

public class CartService
{
    private readonly ICartRepository _repository;
   
    public CartService(ICartRepository repository)
    {
        _repository = repository;
    }

    public ErrorOr<Created> StoreCartItem(CartItem cartItem)
    {
        var sameCartItem = _repository.GetSameCartItem(cartItem);

        if (sameCartItem is null)
            _repository.StoreCartItem(cartItem);
        else
            sameCartItem.IncreaseQuantity();

        _repository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<IEnumerable<CartItem>> GetAllUserCartItems(Guid userId)
    {
        return _repository.GetAllUserCartItems(userId).ToList();
    }
}