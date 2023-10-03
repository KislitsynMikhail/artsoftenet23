namespace WebApplication3;

public class MyFirstMiddleware
{
    private readonly RequestDelegate _next;
    
    public MyFirstMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IConfiguration configuration)
    {
        var value = configuration.GetValue<string>("myKey");

        if (value == "qwerty")
        {
            await httpContext.Response.WriteAsync("Qwerty from First Middleware");
            return;
        }
        
        await _next(httpContext);
    }
}