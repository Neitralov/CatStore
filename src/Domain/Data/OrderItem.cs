namespace Domain.Data;

public class OrderItem
{
    public Guid OrderId { get; private set; }
    public Guid CatId { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice { get; private set; }

    private OrderItem() { }

    public static ErrorOr<OrderItem> Create(
        Guid catId,
        int quantity,
        decimal totalPrice)
    {
        List<Error> errors = new();

        if (quantity < 1)
            errors.Add(Errors.OrderItem.InvalidQuantity);

        if (totalPrice <= 0)
            errors.Add(Errors.OrderItem.InvalidPrice);

        if (errors.Count > 0)
            return errors;

        return new OrderItem
        {
            CatId = catId,
            Quantity = quantity,
            TotalPrice = totalPrice
        };
    }
}