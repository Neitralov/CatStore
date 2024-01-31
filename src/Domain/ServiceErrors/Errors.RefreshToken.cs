namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class RefreshToken
    {
        public static Error NotFound => Error.NotFound(
            code:        "RefreshToken.NotFound",
            description: "Refresh token not found");
        
        public static Error TokenIsInvalid => Error.Validation(
            code:        "RefreshToken.TokenIsInvalid",
            description: "Refresh token is invalid");
    }
}