namespace WebAPI.Controllers;

public class ErrorsController : ControllerBase
{
    [HttpGet("error"), ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error() => Problem();
}