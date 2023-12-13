namespace Domain.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
    }

    public ErrorOr<Created> StoreOrder(Order order, List<CartItem> cartItems)
    {
        _orderRepository.AddOrder(order);
        
        var result = _cartRepository.RemoveCartItems(cartItems);
        if (result is false)
            return Errors.CartItem.NotFound;

        _orderRepository.SaveChanges();
        _cartRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<Order> GetOrder(Guid orderId, Guid userId)
    {
        var result = _orderRepository.GetOrder(orderId);

        if (result is not { } || result.UserId != userId)
            return Errors.Order.NotFound;

        return result;
    }

    public ErrorOr<IEnumerable<Order>> GetAllUserOrders(Guid userId)
    {
        return _orderRepository.GetAllUserOrders(userId).ToList();
    }
}