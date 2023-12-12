namespace WebAPI.Contracts.Order;

public record OrderResponse(
    Guid OrderId,
    DateTime OrderDate,
    decimal TotalPrice);