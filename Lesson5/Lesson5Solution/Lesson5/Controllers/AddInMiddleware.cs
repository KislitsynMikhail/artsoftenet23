using Microsoft.AspNetCore.Mvc;

namespace Lesson5.Controllers;

[Route("hello")]
public class ProductsController : Controller
{
    [HttpGet("world")]
    public IActionResult Details(int id)
    {
        return Ok(ControllerContext.ActionDescriptor.ActionName);
    }
}

