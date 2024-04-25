namespace Domain;

public static partial class Errors
{
    public static class Cat
    {
        public static Error NotFound => Error.NotFound(
            code:        "Cat.NotFound",
            description: "Кот не найден");
        
        public static Error InvalidName => Error.Validation(
            code:        "Cat.InvalidName",
            description: $"Имя кота должно быть не короче {Data.Cat.MinNameLength} " +
                $"символов и не больше {Data.Cat.MaxNameLength} символов");
        
        public static Error InvalidSkinColor => Error.Validation(
            code:        "Cat.InvalidSkinColor",
            description: "Цвет должен быть указан в HEX формате");
        
        public static Error InvalidEyeColor => Error.Validation(
            code:        "Cat.InvalidEyeColor",
            description: "Цвет должен быть указан в HEX формате");
        
        public static Error InvalidEarColor => Error.Validation(
            code:        "Cat.InvalidEarColor",
            description: "Цвет должен быть указан в HEX формате");

        public static Error InvalidCost => Error.Validation(
            code:        "Cat.InvalidCost",
            description: "Кот не может быть бесплатным");

        public static Error InvalidDiscount => Error.Validation(
            code:        "Cat.InvalidDiscount",
            description: "Скидка не может быть отрицательной");

        public static Error AlreadyExists => Error.Validation(
            code:        "Cat.AlreadyExists",
            description: "Кот с таким именем уже существует");
    }
}