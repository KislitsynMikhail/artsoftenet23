using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("operation")]
public class WeatherForecastController : ControllerBase
{
    private static readonly List<User> _users = new List<User>
    {
        new("Петр", "Иванов", 20),
        new("Анна", "Петрова", 18),
        new("Иван", "Петров", 22)
    };

    [HttpGet("arr")]
    public async Task<IActionResult> GetAsync([FromQuery] string[] arr)
    {
        return Ok(arr);
    }

    [HttpPost("arr")]
    public async Task<IActionResult> PostAsync([FromBody] Person person)
    {
        return Ok(person);
    }

    public record Person
    {
        public string[] Arr { get; init; }
    }

    [HttpGet("delay")]
    public async Task<IActionResult> GetAsync([FromQuery] int ms = 100)
    {
        await Task.Delay(ms);
        
        return Ok();
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetDataAsync([FromQuery] string name = null, string surname = null, int minAge = int.MinValue, int maxAge = int.MaxValue, int count = int.MaxValue)
    {
        var userList = _users.Where(x =>
        {
            var isInclude = true;
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (x.Name != name)
                {
                    isInclude = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(surname))
            {
                if (x.Surname != surname)
                {
                    isInclude = false;
                }
            }

            if (minAge > x.Age || x.Age > maxAge)
            {
                isInclude = false;
            }

            return isInclude;
        }).ToList();
        
        return Ok(userList.Take(count));
    }

    [HttpPost("users")]
    public async Task<IActionResult> AddDataAsync([FromBody] User user)
    {
        _users.Add(user);
        
        return Ok();
    }

    [HttpGet("error")]
    public async Task<IActionResult> ErrorAsync()
    {
        throw new Exception();
    }

    public record User(string Name, string Surname, int Age);
}