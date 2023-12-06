namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found");

        public static Error InvalidEmail => Error.Validation(
            code: "User.InvalidEmail",
            description: "This is not an email address");

        public static Error InvalidPassword => Error.Validation(
            code: "User.InvalidPassword",
            description: $"Password must be at least {Data.User.MinPasswordLength} characters long");
        
        public static Error AlreadyExists => Error.Validation(
            code: "User.AlreadyExists",
            description: "User with the same email is already exists");
        
        public static Error PasswordsDontMatch => Error.Validation(
            code: "User.PasswordsDontMatch",
            description: "The password and confirm password fields must match");
    }
}