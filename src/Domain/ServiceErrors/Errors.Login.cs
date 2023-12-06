namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class Login
    {
        public static Error IncorrectEmailOrPassword => Error.Validation(
            code: "User.IncorrectEmailOrPassword",
            description: "Email or password is incorrect");
    }
}