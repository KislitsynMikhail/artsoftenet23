using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Example.Filters.Examples.Filters;

public class DublicateAuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Headers["user-data"].Any())
        {
            var dataUser = context.HttpContext.Request.Headers["user-data"].First();
            if (dataUser != "access")
            {
                context.Result = new UnauthorizedResult();
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
        }
    }
    
    private static bool HasAllowAnonymous(AuthorizationFilterContext context)
    {
        var filters = context.Filters;
        return filters.OfType<IAllowAnonymousFilter>().Any();
    }
}