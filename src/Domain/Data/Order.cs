namespace Domain.Data;

public class Order
{
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public ICollection<OrderItem> OrderItems { get; private set; }
    public decimal TotalPrice { get; private set; }

    private Order() { }

    public static ErrorOr<Order> Create(
        Guid userId,
        ICollection<OrderItem> orderItems,
        decimal totalPrice,
        Guid? orderId = null)
    {
        List<Error> errors = new();

        // Проверки

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