namespace Domain;

public static partial class Errors
{
    public static class Order
    {
        public static Error NotFound => Error.NotFound(
            code:        "Order.NotFound",
            description: "Заказ не найден");

        public static Error EmptyOrder => Error.Validation(
            code:        "Order.EmptyOrder",
            description: "Заказ не может быть пустым");
    }
}