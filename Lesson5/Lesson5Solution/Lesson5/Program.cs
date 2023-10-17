using System.Threading.Tasks;
using Lesson5;
using Lesson5.Feature;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// области "Area"
// User - UserController, AccessUserController
// Company - CompanyController, CreateCompanyController

// области видимости
// BasePublicController : ControllerBase (auth/no auth) - внешние контроллеры для фронта 
// BaseInternalController : ControllerBase - это контроллеры внутри нашей сети (например для тех поддержки)
// BaseExternalController : ControllerBase - для интеграции со сторонними системами 
// BaseDebugController : ControllerBase - если сборка приложения Debug, унодно для отладки
// мы решаем проблему ApiController

// Auth / No Auth

// Add services to the container.

builder.Services
    .AddControllers(opt =>
    {
        opt.Conventions.Add(new GenericControllerRouteConvention());
        opt.FormatterMappings.SetMediaTypeMappingForFormat("xml", new MediaTypeHeaderValue("application/xml"));
    })
    .ConfigureApplicationPartManager(manager => 
    {
        manager.FeatureProviders.Add(new CustomControllerFeature());
        manager.FeatureProviders.Add(new GenericFeatureProvider());
    })
    .AddXmlSerializerFormatters();

// builder.Services.AddControllersWithViews();
// builder.Services.AddMvc();
// builder.Services.AddRazorPages();

builder.Services.AddHostedService<ApplicationPartLogger>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();



app.MapControllers();

app.MapDefaultControllerRoute();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "areas",
    areaName: "Hello",
    pattern: "{area}/{controller=Home}/{action=Index}/{id?}"
);

app.MapFallbackToController("Details","Products");


app.Run();

public class CustomOutputFormatter: OutputFormatter
{
    public CustomOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/vnd.my.customtype+json"));
    }
        
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
    {
        var response = context.HttpContext.Response;
        await response.WriteAsJsonAsync(context.Object);
    }
}