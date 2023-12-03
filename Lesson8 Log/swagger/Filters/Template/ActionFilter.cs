using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Example.Filters.Template;

public class ActionFilter : ActionFilterAttribute  , IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        throw new NotImplementedException();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }
}