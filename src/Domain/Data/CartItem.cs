namespace Domain.Data;

public class CartItem
{
    public Guid UserId { get; private set; }
    public Guid CatId { get; private set; }
    public int Quantity { get; private set; } = 1;

    private CartItem() { }

    public static ErrorOr<CartItem> Create(
        Guid userId,
        Guid catId)
    {
        List<Error> errors = new();

        // Проверки

        if (errors.Count > 0)
            return errors;

        return new CartItem
        {
            UserId = userId,
            CatId = catId
        };
    }

    public void IncreaseQuantity() => Quantity++;
}