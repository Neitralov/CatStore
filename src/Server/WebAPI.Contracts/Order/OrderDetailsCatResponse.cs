namespace WebAPI.Contracts.Order;

public record OrderDetailsCatResponse(
    Guid CatId,
    int Quantity,
    decimal TotalPrice)
{
    /// <example>2</example>
    public int Quantity { get; init; } = Quantity;
    /// <example>200</example>
    public decimal TotalPrice { get; init; } = TotalPrice;
}