namespace WebAPI.Controllers;

/// <inheritdoc />
public class UsersController(UserService userService) : ApiController
{
    /// <summary>Зарегистрировать новый аккаунт</summary>
    /// <response code="204">Регистрация прошла успешно</response>
    /// <response code="400">
    ///     Некорректный адрес электронной почты, слишком короткий пароль, пароли не совпадают, пользователь с такой электронной почтой уже существует
    /// </response>
    [HttpPost]
    [ProducesResponseType(204)]
    public IActionResult CreateUser([Required] CreateUserRequest request)
    {
        ErrorOr<User> requestToUserResult = CreateUserFrom(request);

        if (requestToUserResult.IsError)
            return Problem(requestToUserResult.Errors);

        var user = requestToUserResult.Value;
        ErrorOr<Created> createUserResult = userService.StoreUser(user);

        return createUserResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>Сменить пароль текущего пользователя</summary>
    /// <response code="204">Пароль успешно изменен</response>
    /// <response code="400">Слишком короткий пароль, пароли не совпадают, новый пароль не может совпадать со старым</response>
    [HttpPatch("change-password"), Authorize]
    [ProducesResponseType(204)]
    public IActionResult ChangePassword([Required] ChangeUserPasswordRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<Updated> changeUserPasswordResult = userService.ChangeUserPassword(
            userId,
            request.OldPassword,
            request.NewPassword,
            request.ConfirmNewPassword);

        return changeUserPasswordResult.Match(_ => NoContent(), Problem);
    }
    
    /// <summary>Войти в аккаунт</summary>
    /// <response code="200">Вход произведен успешно</response>
    /// <response code="400">Логин или пароль указан некорректно</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), 200)]
    public IActionResult Login([Required] LoginUserRequest request)
    {
        ErrorOr<string> loginUserResult = userService.Login(request.Email, request.Password);

        return loginUserResult.Match(token => Ok(new LoginUserResponse(token)), Problem);
    }

    private static ErrorOr<User> CreateUserFrom(CreateUserRequest request)
    {
        return Domain.Data.User.Create(
            request.Email,
            request.Password,
            request.ConfirmPassword);
    }
}