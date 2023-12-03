using System.Threading.Tasks;
using Api.Example.Filters.Template;
using Microsoft.AspNetCore.Mvc;

namespace Api.Example.Filters.Examples.Controllers;

[Route("user-filter")]
public class UserFitlerController : Controller
{

    [HttpPost]
    //[SimpleAuthorizationFilter]
    [ResultFilterAsync]
    [ResultFilter]
    public async Task<IActionResult> CreateUserAsync()
    {
        var dd = new bool();
        
        return Ok();
    }
    
    [HttpGet]
    //[DublicateAuthorizationFilter]
    public async Task<IActionResult> GetInfoUserAsync()
    {

        return Ok();
    }
}