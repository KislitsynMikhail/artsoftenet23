using System;
using System.Threading.Tasks;
using Lesson1.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Lesson1.Filters.Examples;

public class Resourse3FilterWithOrder : Attribute, IResourceFilter, IOrderedFilter
{
    public int Order => 1;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        //do something

        context.HttpContext.Response.Headers.Add("custom_Header", new StringValues("da"));
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}

public class Resourse1FilterWithOrder : Attribute, IAsyncResourceFilter, IOrderedFilter
{
    public int Order => 1;
    
    /// <inheritdoc />
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        //do something

        await next();
    }
}

public class Resourse2FilterWithOrder : Attribute, IAsyncResourceFilter, IOrderedFilter
{
    public int Order => 1;

    /// <inheritdoc />
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        //do something

        await next();
    }
}

public class GlobalResultFilter : IAsyncResultFilter
{
    /// <inheritdoc />
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        //do something
        var region = context.HttpContext.Request.Headers["Region"];

        await next();

        switch (region)
        {
            case "en":
            {
                //services convert by EN
                break;
            }
            case "kz":
            {
                //services convert by KZ
                break;
            }
            default:
            {
                //services convert by RUS
                var dd = 1;
                break;
            }
        }
    }
}