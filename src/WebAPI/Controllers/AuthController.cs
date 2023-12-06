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

    private static ErrorOr<User> CreateUserFrom(CreateUserRequest request)
    {
        return Domain.Data.User.Create(
            request.Email,
            request.Password,
            request.ConfirmPassword);
    }
}