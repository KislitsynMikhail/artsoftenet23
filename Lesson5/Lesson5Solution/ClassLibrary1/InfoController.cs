using Microsoft.AspNetCore.Mvc;

namespace ClassLibrary1;

[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}