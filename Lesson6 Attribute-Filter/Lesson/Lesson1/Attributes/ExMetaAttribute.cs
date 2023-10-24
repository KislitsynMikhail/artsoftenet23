using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace Lesson1.Attributes;

/// <summary>
/// Игнорирование логики мониторинга
/// </summary>
public class IgnoreMonitoringAttribute : Attribute
{}

public class MyMiddleware
{
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Обработка Middleware
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var endPointInfo = context.GetEndpoint();
            
        var ignoreMonitoringAttribute = endPointInfo?.Metadata.GetMetadata<IgnoreMonitoringAttribute>();
        if (ignoreMonitoringAttribute != null)
        {
            var d = 1;
            //not logg request info
        }

        await _next(context);
    }
}