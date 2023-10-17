using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson5.Controllers;

public class CustomRequired : ValidationAttribute
{
    public CustomRequired() : base("Is Required")
    {
        
    }
    
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return false;
        }
        
        return true;
    }
}

public interface IExecuteSql
{
    public object[] ExecuteAsync(string sql);
    
    public object[] GetAll();
    
    public object[] GetItemList();
}
public record CreateUserRequest
{
    /// <summary>
    /// Hello
    /// </summary>
    [CustomRequired]
    [JsonPropertyName("userName")]
    public string UserName { get; init; }
}

class Error
{
    public string Message { get; set; }
}

// ----------------------------------------------------------
// доменной логике UserController - UserManager userdal, user_role 
// ----------------------------------------------------------
//                 RoleController - RoleManager roledal
// ----------------------------------------------------------

// CQRS - read/write WriteUserController ReadUserController
// IEndpoint 

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IExecuteSql _logic;

    public UserController(IExecuteSql executeSql)
    {
        _logic = executeSql;
    }

    /// <summary>
    /// Получение заголовка Accept для такой-то бизнес логики
    /// </summary>
    /// <returns></returns>
    [HttpPost("kdnfg")]
    // [ProducesResponseType(typeof(CreateUserRequest), StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateUserAsync([FromBody] CreateUserRequest dto)
    {
        return Ok(new {});
    }

    [HttpGet("data")]
    public IActionResult GetData()
    {
        var result = _logic.GetItemList();
        return Ok(result);
    }
    
    /// <summary>
    /// Получение заголовка Accept для такой-то бизнес логики
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetUserAsync()
    {
        return new ObjectResult(1);
    }
    
    /// <summary>
    /// Получение заголовка Accept для такой-то бизнес логики
    /// </summary>
    /// <returns></returns>
    private string GetHeader()
    {
        return HttpContext.Request.Headers.Accept;
    }
}