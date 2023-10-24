using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lesson1.Filters.Examples;

/// <summary>
/// Второй фитльтр авторизации
/// </summary>
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
}