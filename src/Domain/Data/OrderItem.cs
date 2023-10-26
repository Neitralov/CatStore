namespace Domain.Data;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public Order Order { get; set; }
    public Cat Cat { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}