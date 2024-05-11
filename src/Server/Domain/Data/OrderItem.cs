namespace Domain.Data;

public class OrderItem
{
    [BsonId]
    public Guid OrderItemId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid CatId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal TotalPrice { get; private set; }

    private OrderItem() { }

    public static ErrorOr<OrderItem> Create(
        Cat cat,
        int quantity)
    {
        List<Error> errors = [];

        if (quantity < 1)
            errors.Add(Errors.OrderItem.InvalidQuantity);
        
        var totalPrice = (cat.Cost - cat.Discount) * quantity;

        if (errors.Count > 0)
            return errors;

        return new OrderItem
        {
            CatId = cat.CatId,
            Name = cat.Name,
            Quantity = quantity,
            TotalPrice = totalPrice
        };
    }
}