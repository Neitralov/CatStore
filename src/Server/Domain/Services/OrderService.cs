namespace Domain.Services;

public class OrderService(
    IOrderRepository orderRepository, 
    ICartRepository cartRepository,
    ICatRepository catRepository)
{
    public async Task<ErrorOr<Created>> StoreOrder(Order order)
    {
        await orderRepository.AddOrder(order);
        
        var cartItems = await cartRepository.GetCartItems(order.UserId);
        var result = await cartRepository.RemoveCartItems(cartItems);
        if (result is false)
            return Errors.CartItem.NotFound;

        await orderRepository.SaveChanges();
        await cartRepository.SaveChanges();

        return Result.Created;
    }

    public async Task<ErrorOr<List<OrderItem>>> CreateOrderItemsFromCart(Guid userId)
    {
        var cartItems = await cartRepository.GetCartItems(userId);
        var orderItems = new List<OrderItem>();
        
        foreach (var item in cartItems)
        {
            var cat = await catRepository.GetCat(item.CatId);
            if (cat is null)
                return Errors.Cat.NotFound;
            
            var orderItem = OrderItem.Create(cat, item.Quantity);
            
            if (orderItem.IsError)
                return orderItem.Errors;
            
            orderItems.Add(orderItem.Value);
        }

        return orderItems;
    }

    public async Task<ErrorOr<Order>> GetOrder(Guid orderId, Guid userId)
    {
        var result = await orderRepository.GetOrder(orderId, userId);

        return result is not null ? result : Errors.Order.NotFound;
    }

    public async Task<List<Order>> GetOrders(Guid userId)
    {
        return await orderRepository.GetOrders(userId);
    }
}