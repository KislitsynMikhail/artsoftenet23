using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Example.Filters.Template;

public class ResourseFilter : IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        throw new NotImplementedException();
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        throw new NotImplementedException();
    }
}