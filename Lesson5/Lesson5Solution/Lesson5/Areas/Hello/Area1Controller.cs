using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Lesson5.Areas.Hello;

// [Area("Hello")]
// [Route("Hello/[controller]/[action]")]
// public class Area1Controller : Controller , IActionConstraint
// {
//     public IActionResult Index()
//     {
//         return Ok();
//     }
//
//     public bool Accept(ActionConstraintContext context)
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public int Order { get; }
// }

public class CarControllerAttribute : Attribute, IRouteTemplateProvider
{
    public CarControllerAttribute(int order)
    {
        Order = order;
    }
    
    // путь
    public string Template => "api/car.{format}";
    
    // priority
    public int? Order { get; }
    
    // имя
    public string Name { get; set; } = string.Empty;
}

public class Response
{
    public int Data { get; init; }
}

[Route("api/car.{format}")]
public class Car : Controller
{
    [FormatFilter]
    [HttpGet()]
    public IActionResult Index()
    {
        return Ok(new Response { Data = 1 });
    }
}

// [CarController(2)]
// [ApiController]
// public class Car2Controller : ControllerBase
// {
//     [HttpGet]
//     public IActionResult Index() => Ok("car2");
// }