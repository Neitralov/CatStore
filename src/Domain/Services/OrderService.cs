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
        var result = orderRepository.GetOrder(orderId);

        if (result is not { } || result.UserId != userId)
            return Errors.Order.NotFound;

        return result;
    }

    public ErrorOr<IEnumerable<Order>> GetAllUserOrders(Guid userId)
    {
        return orderRepository.GetAllUserOrders(userId).ToList();
    }
}