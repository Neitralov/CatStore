namespace WebAPI.Contracts.CartItem;

public record CartItemResponse(
    Guid CatId,
    int Quantity)
{
    /// <example>2</example>
    public int Quantity { get; init; } = Quantity;
}