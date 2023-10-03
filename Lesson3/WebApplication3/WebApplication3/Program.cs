
using WebApplication3;

var builder = WebApplication.CreateBuilder(args);
//var builder = WebApplication.CreateBuilder(new WebApplicationOptions { WebRootPath = "otherwwwroot", Args = args });


//builder.Configuration.AddJsonFile("config.json", optional: true);


var app = builder.Build();

var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear(); // удаляем имена файлов по умолчанию
options.DefaultFileNames.Add("papka/other.html"); // добавляем новое имя файла
app.UseDefaultFiles(options); // установка параметров

//app.UseDefaultFiles(); // поддержка страниц html по умолчанию
app.UseStaticFiles(); // по пути к данным в wwwroot
//app.UseDirectoryBrowser();


app.Use(async (context, next) =>
{
    await next();
});

app.UseMiddleware<MyFirstMiddleware>();
app.UseMiddleware<MySecondMiddleware>();

/*app.Map("/qwerty", applicationBuilder =>
{
    applicationBuilder.Run(HandleQwertyAsync);
});*/

/*app.MapWhen(context => context.Request.Path == "/qwerty", applicationBuilder =>
{
    applicationBuilder.Run(HandleQwertyAsync);
});*/

async Task HandleQwertyAsync(HttpContext context)
{
    await Task.Delay(100);
    await context.Response.WriteAsync("gfdgfdsg");
}

app.UseRouting();
app.UseEndpoints(endpoint =>
{
    endpoint.Map("/endpoint", async context =>
    {
        await Task.Delay(100);
        await context.Response.WriteAsync("endpoint");
    });
});


var myRouteHandler = new RouteHandler(async context =>
{
    var dic = context.Request.RouteValues;
    var t = context.Features;
    context.Response.StatusCode = 500;
    await context.Response.WriteAsync($"From route handler: {context.Request.Path}");
});

var routeBuilder = new RouteBuilder(app, myRouteHandler);
//routeBuilder.MapRoute("default", "{controller}/{action}");
//routeBuilder.MapRoute("default", "{controller}/{action=Index}");
routeBuilder.MapRoute("default", "{controller:alpha:minlength(4)}/{action:int}");
app.UseRouter(routeBuilder.Build());




app.Run();