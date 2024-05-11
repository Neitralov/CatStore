namespace Domain.Data;

public class Order
{
    [BsonId]
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public ICollection<OrderItem> OrderItems { get; private set; } = default!;
    public decimal TotalPrice { get; private set; }

    private Order() { }

    public static ErrorOr<Order> Create(
        Guid userId,
        ICollection<OrderItem> orderItems,
        Guid? orderId = null)
    {
        List<Error> errors = [];

        if (orderItems.Count <= 0)
            errors.Add(Errors.Order.EmptyOrder);

        var totalPrice = orderItems.Sum(item => item.TotalPrice);

        if (errors.Count > 0)
            return errors;

        return new Order
        {
            OrderId = orderId ?? Guid.NewGuid(),
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            OrderItems = orderItems,
            TotalPrice = totalPrice
        };
    }
}