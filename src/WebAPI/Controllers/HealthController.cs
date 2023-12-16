namespace WebAPI.Controllers;

/// <inheritdoc />
public class HealthController : ApiController
{
    /// <summary></summary>>
    [HttpGet]
    public IActionResult CheckHealth() => Ok();
}