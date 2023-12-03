using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Destructurama.Attributed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc
        .Enrich.WithThreadId()
        .Enrich.FromLogContext()
        .AuditTo.Sink<SerilogSink>()
        .Filter.With<SerilogFilter>()
        .Enrich.With<SerilogEnricher>()
        .WriteTo.Console(
            LogEventLevel.Information,
            outputTemplate: 
"{Timestamp:HH:mm:ss:ms} LEVEL:[{Level}]| THREAD:|{ThreadId}| Test:|{Test}| {Message}{NewLine}{Exception}")
    , preserveStaticLogger: false, writeToProviders: false);

builder.Host.ConfigureAppConfiguration((context, config) =>
{
    if (context.HostingEnvironment.IsDevelopment())
    {
        // таким образом мы можем использовать
        // промежуточный резльтат для дальнейшего конфигурирования
        var buildResult = config.Build();
    }
    
    var d = 1;
});
// Собираем все DI зависимости
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SettingFileApiConnecti>(
    builder.Configuration.GetSection(nameof(SettingFileApiConnecti)));

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(nameof(AppSettings)));


builder.Services.AddLogging();
#region Урок по Filters
//builder.Services.AddControllers(x=>x.Filters.Add(new DublicateAuthorizationFilter()));

#endregion
// builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>(x
//     => new ConfigureSwaggerOptions());
// builder.Services.AddVersionedApiExplorer(options =>
// {
//     options.GroupNameFormat = "'v'VVV";
//     options.SubstituteApiVersionInUrl = true;
// });

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
            
Log.Information("Starting up!");

builder.Services.AddSingleton<AppSettings>();
var app = builder.Build();

var configure = app.Services.GetRequiredService<IConfiguration>();
var value = configure.GetSection("AllowedHosts").Get<object>();


LogContext.PushProperty("Test", "some super data");
Log.Information("Abanking the best");
app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();

var user = new UserInfo
{
    Id = 1,
    Password = new Password
    {
        Value = "password"
    }
};
Log.Information("User : {0} register", user.Id);



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    //var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        // foreach (var description in provider.ApiVersionDescriptions)
        // {
        //     options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.ApiVersion.ToString());
        // }
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.DocumentTitle = "Default REST API";
        options.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.Run();

#region FileAPi
public interface IConnectFileApi
{
    
}

internal class ConnectFileApi : IConnectFileApi
{
    private readonly SettingFileApiConnecti _settingFileApiConnecti;
    private readonly IHttpClientFactory _clientFactory;

    public ConnectFileApi(SettingFileApiConnecti settingFileApiConnecti, IHttpClientFactory clientFactory)
    {
        _settingFileApiConnecti = settingFileApiConnecti;
        _clientFactory = clientFactory;
    }

    public async Task<IFileInfo> GetFileInfoAsync(Guid id)
    {
        return new NotFoundFileInfo("");
    }
}

#endregion

public class SettingFileApiConnecti
{
    private readonly IConfiguration _configuration;

    public SettingFileApiConnecti(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    internal string ConnectionUrl { get; }
}

public class BaseAppSetting
{
    private readonly IConfiguration _configuration;

    public BaseAppSetting(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}


public class AppSettings : BaseAppSetting
{
    private readonly IConfiguration _configuration;

    public AppSettings(IConfiguration configuration) : base(configuration)
    {
        _configuration = configuration;
    }

    public string AllowedHosts => _configuration.GetSection(nameof(AllowedHosts)).Get<string>();
}

public class SerilogSink : ILogEventSink 
{
    public void Emit(LogEvent logEvent)
    {
        Console.WriteLine(logEvent.MessageTemplate);
    }
    
   
}

public class SerilogFilter : ILogEventFilter
{
    public bool IsEnabled(LogEvent logEvent)
    {
        return true;
    }
}

public class SerilogEnricher : ILogEventEnricher 
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("EventId", "Abanking"));
    }
}

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{

    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc( "hello wrold", CreateInfoForApiVersion());
    }
    
    /// <summary>
    /// Создание описания для заголвока страницы
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    private OpenApiInfo CreateInfoForApiVersion()
    {
        var info = new OpenApiInfo()
        {
            Title = "Hello world",
            Description = "This is description",
            Contact = new OpenApiContact { Name = "This is Contact", Email = "", Url = new Uri("https://abanking.ru/") },
            License = new OpenApiLicense { Name = "Tis is License", Url = new Uri("https://abanking.ru/") },
            TermsOfService = new Uri("https://abanking.ru/")
        };

        return info;
    }
}

public class Password
{
    [LogMasked]
    public string Value { get; init; }
}

class UserInfo
{
    public int Id { get; init; }
    
    [LogMasked]
    public Password Password { get; init; }
}