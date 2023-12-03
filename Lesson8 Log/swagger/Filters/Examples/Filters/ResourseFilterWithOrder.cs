using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Example.Filters.Examples.Filters;

public class ResourseFilterWithOrder :  IResourceFilter, IOrderedFilter
{
    public int Order => 1;
    
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        throw new NotImplementedException();
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        throw new NotImplementedException();
    }
}