using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Example.Filters.Template
{
    public class ResultFilter : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var dd = new bool();

        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var dd = new bool();
        }
    }
}