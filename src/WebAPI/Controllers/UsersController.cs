namespace WebAPI.Controllers;

public class UsersController : ApiController
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        ErrorOr<User> requestToUserResult = CreateUserFrom(request);

        if (requestToUserResult.IsError)
            return Problem(requestToUserResult.Errors);

        var user = requestToUserResult.Value;
        ErrorOr<Created> createUserResult = _userService.StoreUser(user);

        return createUserResult.Match(_ => NoContent(), Problem);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUserRequest request)
    {
        ErrorOr<string> loginUserResult = _userService.Login(request.Email, request.Password);

        return loginUserResult.Match(token => Ok(new LoginUserResponse(token)), Problem);
    }

    [HttpDelete, Authorize]
    public IActionResult DeleteAccount()
    {
        var userId = GetUserGuid();

        ErrorOr<Deleted> deleteUserResult = _userService.DeleteUserById(userId);

        return deleteUserResult.Match(_ => NoContent(), Problem);
    }

    [HttpPatch("change-password"), Authorize]
    public IActionResult ChangePassword(ChangeUserPasswordRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<Updated> changeUserPasswordResult = _userService.ChangeUserPassword(
            userId,
            request.OldPassword,
            request.NewPassword,
            request.ConfirmNewPassword);

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