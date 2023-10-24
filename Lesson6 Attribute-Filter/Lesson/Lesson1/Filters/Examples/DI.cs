using System;
using System.Threading.Tasks;
using Lesson1.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Lesson1.Filters.Examples;

/// <summary>
/// Первый способ регистрации
/// </summary>
public class Action1DIFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //do something

        var instance = context.HttpContext.RequestServices.GetRequiredService<IService>();
        var dd = instance.GetRandomNumber();
        
        await next();
    }
}


/// <summary>
/// Второй способ регистрации
/// </summary>
public class Action2DIFilter : IAsyncActionFilter
{
    private readonly IService _service;

    public Action2DIFilter(IService service)
    {
        _service = service;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //do something

        var instance = context.HttpContext.RequestServices.GetRequiredService<IService>();
        var dd = instance.GetRandomNumber();
        
        await next();
    }
}


/// <summary>
/// Третий способ регистрации
/// </summary>
public class Action3DIFilter : IAsyncActionFilter
{
    private readonly IService _service;
    private string _name;

    public Action3DIFilter(IService service, string name)
    {
        _service = service;
        _name = name;
    }
    
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //do something

        var instance = context.HttpContext.RequestServices.GetRequiredService<IService>();
        var dd = instance.GetRandomNumber();

        await next();
    }
}

/// <summary>
/// Четвертый способ регистрации
/// </summary>
public class Action4DIAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;
 
    private readonly string[] _apiRoleArray;
 
    public Action4DIAttribute(string[] apiRoleArray)
    {
        _apiRoleArray = apiRoleArray;
    }
 
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var clientApiService = serviceProvider.GetRequiredService<IService>();
        var clientApiAuthenticationFilter = new Action4DIFilter(_apiRoleArray, clientApiService);
        return clientApiAuthenticationFilter;
    }
}

public class Action4DIFilter : IAsyncActionFilter
{
    private readonly IService _service;
    private string[] _apiRoleArray;

    public Action4DIFilter(string[] apiRoleArray ,IService service)
    {
        _apiRoleArray = apiRoleArray;
        _service = service;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //do something

        var d = _service.GetRandomNumber();
        
        await next();
    }
}
