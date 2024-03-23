namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class CartItem
    {
        public static Error NotFound => Error.NotFound(
            code:        "CartItem.NotFound",
            description: "Cart item not found");

        public static Error InvalidQuantity => Error.Validation(
            code:        "CartItem.InvalidQuantity",
            description: "Cart item quantity cannot be less than one");
    }
}