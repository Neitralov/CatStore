namespace WebAPI.Controllers;

public class ErrorController : ControllerBase
{
    [HttpGet("error"), ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error() => Problem();
}