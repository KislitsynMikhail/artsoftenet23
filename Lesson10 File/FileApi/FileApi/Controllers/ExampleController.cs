using FileApi.Example;
using Microsoft.AspNetCore.Mvc;

namespace FileApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    [HttpGet("drive-info")]
    public IActionResult CreateFile()
    {
        DriveExample.WriteDriveInfoInConsole();
        return Ok();
    }
}