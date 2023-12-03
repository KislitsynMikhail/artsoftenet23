using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestApi;


public class BaseWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        
        // Подключаем DI
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        // здесь можно разместить подключение различных кастомных штук,
        // например чтение ошибок, инициализацию сидов для тестовой базы данных и тд
        return host;
    }

    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    /// <returns></returns>
    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder();
        return builder;
    }
}