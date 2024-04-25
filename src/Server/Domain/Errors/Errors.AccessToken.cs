namespace Domain;

public static partial class Errors
{
    public static class AccessToken
    {
        public static Error NotFound => Error.NotFound(
            code:        "AccessToken.NotFound",
            description: "Токен доступа не найден");

        public static Error TokenIsInvalid => Error.Validation(
            code:        "AccessToken.TokenIsInvalid",
            description: "Токен доступа недействителен");
    }
}