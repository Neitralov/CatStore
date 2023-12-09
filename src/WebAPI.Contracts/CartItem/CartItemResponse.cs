namespace WebAPI.Contracts.CartItem;

public record CartItemResponse(
    Guid CatId,
    int Quantity);