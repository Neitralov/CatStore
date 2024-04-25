namespace Domain;

public static partial class Errors
{
    public static class RefreshToken
    {
        public static Error NotFound => Error.NotFound(
            code:        "RefreshToken.NotFound",
            description: "Токен обновления не найден");
        
        public static Error TokenIsInvalid => Error.Validation(
            code:        "RefreshToken.TokenIsInvalid",
            description: "Токен обновления недействителен");
    }
}