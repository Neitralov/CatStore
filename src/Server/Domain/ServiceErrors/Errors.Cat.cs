namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class Cat
    {
        public static Error NotFound => Error.NotFound(
            code:        "Cat.NotFound",
            description: "Cat not found");
        
        public static Error InvalidName => Error.Validation(
            code:        "Cat.InvalidName",
            description: $"Cat name must be at least {Data.Cat.MinNameLength} " +
                $"characters long and at most {Data.Cat.MaxNameLength} characters long.");
        
        public static Error InvalidSkinColor => Error.Validation(
            code:        "Cat.InvalidSkinColor",
            description: "Сat must have the color in HEX format.");
        
        public static Error InvalidEyeColor => Error.Validation(
            code:        "Cat.InvalidEyeColor",
            description: "Сat must have the color in HEX format.");
        
        public static Error InvalidCost => Error.Validation(
            code:        "Cat.InvalidCost",
            description: "Сat can't be free.");

        public static Error AlreadyExists => Error.Validation(
            code:        "Cat.AlreadyExists",
            description: "Cat with the same name is already exists");
    }
}