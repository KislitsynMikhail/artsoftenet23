using Api.Controllers.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/v1/test")]
public class ForTestController : ControllerBase
{
    /// <summary>
    /// Тестовые контроллер для интеграционных тестов
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200)]
    public IActionResult GetTestData()
    {
        return Ok(new TestGetResponse { IsSuccessed = true, RandomNumber = 1 });
    }
}