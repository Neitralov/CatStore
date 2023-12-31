namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class Order
    {
        public static Error NotFound => Error.NotFound(
            code:        "Order.NotFound",
            description: "Order not found");

        public static Error InvalidPrice => Error.Validation(
            code:        "Order.InvalidPrice",
            description: "Order cannot be free");

        public static Error EmptyOrder => Error.Validation(
            code:        "Order.EmptyOrder",
            description: "Order cannot be empty");
    }
}