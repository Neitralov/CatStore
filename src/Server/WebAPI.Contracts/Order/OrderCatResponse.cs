namespace WebAPI.Contracts.Order;

public record OrderCatResponse(
    Guid CatId,
    string Name,
    int Quantity,
    decimal TotalPrice)
{
    // <example>Персик</example>
    public string Name { get; init; } = Name;
    /// <example>2</example>
    public int Quantity { get; init; } = Quantity;
    /// <example>200</example>
    public decimal TotalPrice { get; init; } = TotalPrice;
}