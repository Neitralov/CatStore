namespace WebAPI.Controllers;

public class HealthController : ApiController
{
    [HttpGet]
    public IActionResult CheckHealth() => Ok();
}