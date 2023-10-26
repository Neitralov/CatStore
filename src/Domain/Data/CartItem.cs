namespace Domain.Data;

public class CartItem
{
    public int CartItemId { get; set; }
    public User User { get; set; }
    public Cat Cat { get; set; }
    public int Quantity { get; set; }
}