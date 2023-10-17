

using System;
using Microsoft.AspNetCore.Mvc;

namespace Lesson5.Controllers;

/// <summary>
/// 
/// </summary>
[Route("[controller]/[action]")]
public abstract class Products0Controller : Controller
{
    private readonly IServiceProvider _serviceProvider;

    public Products0Controller(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    // Matches '/Products0/List'
    [HttpGet] 
    public IActionResult List()
    {
        return Ok();
    }
 
    // Matches '/Products0/Edit/{id}'
    [HttpGet("{id}")] 
    public IActionResult Edit(int id)
    {
        return Ok();
    }
}

public abstract class Products20Controller : Controller
{
    // Matches '/Products20/List'
    [HttpGet("[controller]/[action]")]  
    public IActionResult List()
    {
        return Ok();
    }
 
    // Matches '/Products20/Edit/{id}'
    [HttpGet("[controller]/[action]/{id}")]  
    public IActionResult Edit(int id)
    {
        return Ok();
    }
}

// Контроллер родитель
[ApiController]
[Route("api/[controller]/[action]")]
public abstract class MyBase2Controller : ControllerBase
{
}
 
// Контроллер наследник
public abstract class Products11Controller : MyBase2Controller
{
    // /api/products11/list
    [HttpGet]                     
    public IActionResult List()
    {
        return Ok();
    }
    
    //    /api/products11/edit/3
    [HttpGet("{id}")]           
    public IActionResult Edit(int id)
    {
        return Ok();
    }
}