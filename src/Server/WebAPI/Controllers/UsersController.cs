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
        ErrorOr<(string accessToken, string refreshToken)> loginUserResult = userService.Login(request.Email, request.Password);

        if (loginUserResult.IsError)
            return Problem(loginUserResult.Errors);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Path = "/api/users",
            Expires = DateTime.UtcNow.AddDays(RefreshTokenSession.ExpiresInDays)
        };

        var accessToken = loginUserResult.Value.accessToken;
        var refreshToken = loginUserResult.Value.refreshToken;

        Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        return Ok(new LoginUserResponse(accessToken));
    }

    /// <summary>Обновить access и refresh токены</summary>
    /// <response code="200">Токены обновлены успешно</response>
    /// <response code="400">Срок действия refresh токена истек, refresh токен недействителен, access токен недействителен</response>
    /// <response code="404">Владелец токена (пользователь) не найден</response>
    [HttpPost("refresh-tokens")]
    [ProducesResponseType(typeof(LoginUserResponse), 200)]
    public IActionResult LoginByTokens([FromHeader] string? expiredAccessToken)
    {
        var refreshToken = HttpContext.Request.Cookies["RefreshToken"];

        ErrorOr<(string accessToken, string refreshToken)> refreshTokensResult = userService.RefreshTokens(expiredAccessToken, refreshToken);

        if (refreshTokensResult.IsError)
            return Problem(refreshTokensResult.Errors);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Path = "/api/users",
            Expires = DateTime.UtcNow.AddDays(RefreshTokenSession.ExpiresInDays)
        };

        var newAccessToken = refreshTokensResult.Value.accessToken;
        var newRefreshToken = refreshTokensResult.Value.refreshToken;

        Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);
        return Ok(new LoginUserResponse(newAccessToken));
    }

    private static ErrorOr<User> CreateUserFrom(CreateUserRequest request)
    {
        return Domain.Data.User.Create(
            request.Email,
            request.Password,
            request.ConfirmPassword);
    }
}