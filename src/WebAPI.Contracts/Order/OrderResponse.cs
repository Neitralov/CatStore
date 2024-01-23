namespace WebAPI.Contracts.Order;

public record OrderResponse(
    Guid OrderId,
    DateTime OrderDate,
    decimal TotalPrice)
{
    /// <example>200</example>
    public decimal TotalPrice { get; init; } = TotalPrice;
}