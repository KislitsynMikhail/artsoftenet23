using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Lesson5.Controllers;

[ApiController]
[Route("[controller]")] // WeatherForecastController -контроллер
public abstract class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    // IEnumerable<WeatherForecast> - результат действия
    public IActionResult Get() // Get() - действие
    {
        var Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        
        return new OkObjectResult(result);
    }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}