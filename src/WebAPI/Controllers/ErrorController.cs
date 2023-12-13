namespace WebAPI.Controllers;

public class ErrorController : ControllerBase
{
    [HttpGet("error")]
    public IActionResult Error() => Problem();
}