namespace WebAPI.Contracts.CartItem;

public record UpdateCartItemQuantityRequest(
    Guid CatId,
    int Quantity);