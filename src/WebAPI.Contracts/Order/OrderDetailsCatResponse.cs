namespace WebAPI.Contracts.Order;

public record OrderDetailsCatResponse(
    Guid CatId,
    int Quantity,
    decimal TotalPrice);