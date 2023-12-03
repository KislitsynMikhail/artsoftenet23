using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Котнроллер для работы с пользователя
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UserController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserAsync([FromQuery] int id)
    {
        return Ok();
    }
}