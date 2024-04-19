namespace WebAPI.Controllers;

/// <inheritdoc />
[Route("/api/users"), Tags("Users")]
public class UsersController(UserService userService) : ApiController
{
    /// <summary>Зарегистрировать новый аккаунт</summary>
    /// <response code="204">Регистрация прошла успешно</response>
    /// <response code="400">
    ///     Некорректный адрес электронной почты, слишком короткий пароль, пароли не совпадают, пользователь с такой электронной почтой уже существует
    /// </response>
    [HttpPost]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CreateUser([Required] CreateUserRequest request)
    {
        ErrorOr<User> requestToUserResult = CreateUserFrom(request);

        if (requestToUserResult.IsError)
            return Problem(requestToUserResult.Errors);

        var user = requestToUserResult.Value;
        ErrorOr<Created> createUserResult = await userService.StoreUser(user);

        return createUserResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>Сменить пароль текущего пользователя</summary>
    /// <response code="204">Пароль успешно изменен</response>
    /// <response code="400">Слишком короткий пароль, пароли не совпадают, новый пароль не может совпадать со старым</response>
    [HttpPatch("change-password"), Authorize]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ChangePassword([Required] ChangeUserPasswordRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<Updated> changeUserPasswordResult = await userService.ChangeUserPassword(
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
    public async Task<IActionResult> Login([Required] LoginUserRequest request)
    {
        ErrorOr<TokensPair> loginUserResult = await userService.Login(request.Email, request.Password);

        if (loginUserResult.IsError)
            return Problem(loginUserResult.Errors);

        var accessToken = loginUserResult.Value.AccessToken;
        var refreshToken = loginUserResult.Value.RefreshToken;

        return Ok(new LoginUserResponse(accessToken, refreshToken));
    }

    /// <summary>Обновить access и refresh токены</summary>
    /// <response code="200">Токены обновлены успешно</response>
    /// <response code="400">Срок действия refresh токена истек, refresh токен недействителен, access токен недействителен</response>
    /// <response code="404">Владелец токена (пользователь) не найден</response>
    [HttpPost("refresh-tokens")]
    [ProducesResponseType(typeof(LoginUserResponse), 200)]
    public async Task<IActionResult> LoginByTokens([Required] RefreshUserTokensRequest request)
    {
        ErrorOr<TokensPair> refreshTokensResult = await userService.RefreshTokens(request.ExpiredAccessToken,  request.RefreshToken);

        if (refreshTokensResult.IsError)
            return Problem(refreshTokensResult.Errors);

        var newAccessToken = refreshTokensResult.Value.AccessToken;
        var newRefreshToken = refreshTokensResult.Value.RefreshToken;
        
        return Ok(new LoginUserResponse(newAccessToken, newRefreshToken));
    }

    private static ErrorOr<User> CreateUserFrom(CreateUserRequest request)
    {
        return Domain.Data.User.Create(
            request.Email,
            request.Password,
            request.ConfirmPassword);
    }
}