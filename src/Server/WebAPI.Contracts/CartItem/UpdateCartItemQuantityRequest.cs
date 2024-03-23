namespace WebAPI.Contracts.CartItem;

public record UpdateCartItemQuantityRequest(
    Guid CatId,
    int Quantity)
{
    /// <example>3</example>
    public int Quantity { get; init; } = Quantity;
}