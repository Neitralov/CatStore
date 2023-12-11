namespace Domain.Data;

public class CartItem
{
    public Guid UserId { get; private set; }
    public Guid CatId { get; private set; }
    public int Quantity { get; private set; } = 1;

    private CartItem() { }

    public static ErrorOr<CartItem> Create(
        Guid userId,
        Guid catId,
        int quantity = 1)
    {
        List<Error> errors = new();

        // Проверки

        if (errors.Count > 0)
            return errors;

        return new CartItem
        {
            UserId = userId,
            CatId = catId,
            Quantity = quantity
        };
    }

    public void IncreaseQuantity() => Quantity++;

    public ErrorOr<Updated> UpdateQuantity(int quantity)
    {
        if (quantity < 1)
            return Errors.CartItem.InvalidQuantity;

        Quantity = quantity;

        return Result.Updated;
    }
}