namespace WebAPI.Controllers;

public class ErrorController : ControllerBase
{
    [HttpGet]
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }    
}