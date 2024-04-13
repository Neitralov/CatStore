namespace Domain.Services;

public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
{
    public ErrorOr<Created> StoreOrder(Order order, List<CartItem> cartItems)
    {
        orderRepository.AddOrder(order);
        
        var result = cartRepository.RemoveCartItems(cartItems);
        if (result is false)
            return Errors.CartItem.NotFound;

        orderRepository.SaveChanges();
        cartRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<Order> GetOrder(Guid orderId, Guid userId)
    {
        var result = orderRepository.GetOrder(orderId, userId);

        return result is not null ? result : Errors.Order.NotFound;
    }

    public ErrorOr<IEnumerable<Order>> GetOrders(Guid userId)
    {
        return orderRepository.GetOrders(userId).ToList();
    }
}