namespace WebAPI.Contracts.Order;

public record OrderResponse(
    Guid OrderId,
    DateTime OrderDate,
    decimal TotalPrice,
    IEnumerable<OrderCatResponse> Cats)
{
    /// <example>200</example>
    public decimal TotalPrice { get; init; } = TotalPrice;
}