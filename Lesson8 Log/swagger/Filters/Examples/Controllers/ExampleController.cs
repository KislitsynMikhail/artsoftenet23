using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Api.Example.Filters.Examples.Controllers;

[Route("internal/example")]
public class ExampleController : Controller
{
    [TypeFilter(typeof(AllowAnonymousFilter))]
    [HttpPost("common-use-case")]
    public async Task<IActionResult> TestCommonUseCase()
    {
        return Ok(new {Item = 1});
    }
}