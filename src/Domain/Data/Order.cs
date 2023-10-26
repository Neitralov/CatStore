namespace Domain.Data;

public class Order
{
    public int OrderId { get; set; }
    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
}