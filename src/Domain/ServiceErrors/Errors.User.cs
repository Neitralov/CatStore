namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error AlreadyExists => Error.Validation(
            code: "User.AlreadyExists",
            description: "User with the same email is already exists");
        
        public static Error PasswordsDontMatch => Error.Validation(
            code: "User.PasswordsDontMatch",
            description: "The password and confirm password fields must match");
    }
}