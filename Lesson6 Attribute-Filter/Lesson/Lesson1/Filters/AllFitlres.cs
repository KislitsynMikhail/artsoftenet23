using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lesson1.Filters;

public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    /// <inheritdoc />
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        //do something
        var dd = 1;
    }
}

public class ResourceFilter : IAsyncResourceFilter
{
    private string _version;

    public ResourceFilter(string version)
    {
        _version = version;
    }
    
    /// <inheritdoc />
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        //do something
        var dd = _version;

        await next();
        
        dd = _version;
    }
}

public class ActionFilter : IAsyncActionFilter
{
    public ActionFilter()
    {
        
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //do something
        var dd = 1;

        await next();

        dd = 1;
    }
}

public class ResultFilter : IAsyncResultFilter
{
    /// <inheritdoc />
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        //do something
        var dd = 1;
        
        await next();
        
        dd = 1;
    }
}

public class ExceptionFilter : IAsyncExceptionFilter
{
    /// <inheritdoc />
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        //do something
        var dd = 1;
    }
}