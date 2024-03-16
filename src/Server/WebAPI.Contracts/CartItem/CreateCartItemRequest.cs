namespace WebAPI.Contracts.CartItem;

public record CreateCartItemRequest(
    Guid CatId,
    int Quantity = 1)
{
    /// <example>1</example>
    public int Quantity { get; init; } = Quantity;
}