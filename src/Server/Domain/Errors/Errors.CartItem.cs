namespace Domain;

public static partial class Errors
{
    public static class CartItem
    {
        public static Error NotFound => Error.NotFound(
            code:        "CartItem.NotFound",
            description: "Элемент корзины не найден");

        public static Error InvalidQuantity => Error.Validation(
            code:        "CartItem.InvalidQuantity",
            description: "Количество товара не может быть меньше единицы");
    }
}