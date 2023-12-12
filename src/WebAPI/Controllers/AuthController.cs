namespace WebAPI.Controllers;

[Route("api/Users")]
public class AuthController : ApiController
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        ErrorOr<User> requestToUserResult = CreateUserFrom(request);

        if (requestToUserResult.IsError)
            return Problem(requestToUserResult.Errors);

        var user = requestToUserResult.Value;
        ErrorOr<Created> createUserResult = _authService.StoreUser(user);

        return createUserResult.Match(_ => NoContent(), Problem);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUserRequest request)
    {
        ErrorOr<string> loginUserResult = _authService.Login(request.Email, request.Password);

        return loginUserResult.Match(token => Ok(new LoginUserResponse(token)), Problem);
    }

    [HttpDelete, Authorize]
    public IActionResult DeleteAccount()
    {
        var userId = GetUserGuid();

        ErrorOr<Deleted> deletedUserResult = _authService.DeleteUserById(userId);

        return deletedUserResult.Match(_ => NoContent(), Problem);
    }

    [HttpPatch("change-password"), Authorize]
    public IActionResult ChangePassword(ChangeUserPasswordRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<Success> changeUserPasswordResult = _authService.ChangeUserPassword(userId, request.OldPassword, request.NewPassword, request.ConfirmNewPassword);

        return changeUserPasswordResult.Match(_ => NoContent(), Problem);
    }

    private static ErrorOr<User> CreateUserFrom(CreateUserRequest request)
    {
        return Domain.Data.User.Create(
            request.Email,
            request.Password,
            request.ConfirmPassword);
    }
}