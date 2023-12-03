using FileApi.UseCase;
using FileApi.UseCase.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IFileUseCaseService, FileUseCaseService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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