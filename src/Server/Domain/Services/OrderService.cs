namespace Domain.Services;

public class OrderService(
    IOrderRepository orderRepository, 
    ICartRepository cartRepository,
    ICatRepository catRepository)
{
    public ErrorOr<Created> StoreOrder(Order order)
    {
        orderRepository.AddOrder(order);
        
        var cartItems = cartRepository.GetCartItems(order.UserId).ToList();
        var result = cartRepository.RemoveCartItems(cartItems);
        if (result is false)
            return Errors.CartItem.NotFound;

        orderRepository.SaveChanges();
        cartRepository.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<List<OrderItem>> CreateOrderItemsFromCart(Guid userId)
    {
        var cartItems = cartRepository.GetCartItems(userId).ToList();
        var orderItems = new List<OrderItem>();
        
        foreach (var item in cartItems)
        {
            var cat = catRepository.GetCat(item.CatId);
            if (cat is null)
                return Errors.Cat.NotFound;
            
            var orderItem = OrderItem.Create(
                catId:      cat.CatId,
                catName:    cat.Name,
                quantity:   item.Quantity,
                totalPrice: (cat.Cost - cat.Discount) * item.Quantity);
            
            if (orderItem.IsError)
                return orderItem.Errors;
            
            orderItems.Add(orderItem.Value);
        }

        return orderItems;
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