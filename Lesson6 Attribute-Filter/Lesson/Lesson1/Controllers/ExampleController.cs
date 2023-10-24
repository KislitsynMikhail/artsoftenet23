using System;
using System.Threading.Tasks;
using Lesson1.Attributes;
using Lesson1.Controllers.Base;
using Lesson1.Filters.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson1.Controllers;

[Route("example")]
public class ExampleController : BaseProjectController
{
    [HttpPost("ignore")]
    [IgnoreMonitoring]
    public async Task<IActionResult> GetIgnoreAsync([FromBody]MyClass dto)
    {

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> GetAsync(
        [FromForm] IFormFile file)
    {
        return Ok();
    }


    [HttpGet("di")]
    [ServiceFilter(typeof(Action2DIFilter))]
    [TypeFilter(typeof(Action3DIFilter), Arguments = new object[]{"StringConstr"})]
    public async Task<IActionResult> GetByDIAsync()
    {
        return Ok();
    }
}