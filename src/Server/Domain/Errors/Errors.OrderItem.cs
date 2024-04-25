namespace Domain;

public static partial class Errors
{
    public static class OrderItem
    {
        public static Error InvalidQuantity => Error.Validation(
            code:        "OrderItem.InvalidQuantity",
            description: "Количество элементов в заказе не может быть меньше одного");
    }
}