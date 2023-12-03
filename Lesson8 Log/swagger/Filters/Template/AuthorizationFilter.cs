using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Example.Filters.Template;

public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}