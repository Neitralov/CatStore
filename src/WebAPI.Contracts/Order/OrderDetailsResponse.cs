namespace WebAPI.Contracts.Order;

public record OrderDetailsResponse(
    Guid OrderId,
    DateTime OrderDate,
    decimal TotalPrice,
    IEnumerable<OrderDetailsCatResponse> Cats);