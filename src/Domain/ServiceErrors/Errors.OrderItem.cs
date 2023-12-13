namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class OrderItem
    {
        public static Error InvalidQuantity => Error.Validation(
            code:        "OrderItem.InvalidQuantity",
            description: "Order item quantity cannot be less than one");
        
        public static Error InvalidPrice => Error.Validation(
            code:        "OrderItem.InvalidPrice",
            description: "Order item cannot be free");
    }
}