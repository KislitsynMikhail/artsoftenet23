using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ApbBinding.Example1;

[ApiController]
[Route("example-1")]
public class Example1Controller : ControllerBase
{
    public Example1Controller()
    {
        
    }
    
    [HttpGet("hello")]
    public IActionResult Index([FromQuery] string value)
    {
        return Ok();
    }
    
    
    [HttpPost("2/{id}")]
    public IActionResult Index2([FromServices] Index2BodyRequest index2BodyRequest, [FromRoute(Name = "id")] string idFromRoute)
    {
        return Ok();
    }
}

public class Index2BodyRequest
{
    [Required]
    public Phone Phone { get; init; }
}