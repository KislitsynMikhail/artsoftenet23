using Microsoft.AspNetCore.Mvc;

namespace Lesson5.Controllers;

public abstract class Test
{
    public int Count { get; init; }
}

public class Test1 : Test
{
}

public class Test2 : Test
{
}

public class GenericController<T> : ControllerBase
    where T : Test
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(typeof(T).Name);
    }
}