namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class AccessToken
    {
        public static Error NotFound => Error.NotFound(
            code:        "AccessToken.NotFound",
            description: "Access token not found");

        public static Error TokenIsInvalid => Error.Validation(
            code: "AccessToken.TokenIsInvalid",
            description: "Access token is invalid");
    }
}