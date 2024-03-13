namespace Domain.ServiceErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound => Error.NotFound(
            code:        "User.NotFound",
            description: "Пользователь не найден");

        public static Error InvalidEmail => Error.Validation(
            code:        "User.InvalidEmail",
            description: "Email указан некорректно");

        public static Error InvalidPassword => Error.Validation(
            code:        "User.InvalidPassword",
            description: $"Пароль должен быть не короче {Data.User.MinPasswordLength} символов");
        
        public static Error AlreadyExists => Error.Validation(
            code:        "User.AlreadyExists",
            description: "Пользователь с таким email уже существует");
        
        public static Error PasswordsDontMatch => Error.Validation(
            code:        "User.PasswordsDontMatch",
            description: "Пароли в полях должны совпадать");

        public static Error NewAndOldPasswordAreTheSame => Error.Validation(
            code:        "User.NewAndOldPasswordAreTheSame",
            description: "Новый пароль не может совпадать со старым");
    }
}