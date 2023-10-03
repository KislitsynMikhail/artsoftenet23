namespace WebApplication3;

public class MySecondMiddleware
{
    private readonly RequestDelegate _next;

    public MySecondMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);
    }
}