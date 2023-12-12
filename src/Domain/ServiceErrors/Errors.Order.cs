namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class Order
    {
        public static Error NotFound => Error.NotFound(
            code: "Order.NotFound",
            description: "Order not found");
    }
}