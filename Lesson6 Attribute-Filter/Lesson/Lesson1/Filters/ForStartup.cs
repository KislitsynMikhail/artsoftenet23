using Lesson1.Filters;
using Lesson1.Filters.Examples;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Filters;

public static class ForStartup
{
    public static IServiceCollection FiltersLesson(this IServiceCollection services)
    {
        #region очередность выполнения в зависимости от типа фильтра

        services.AddControllers(x =>
        {
            x.Filters.Add<AuthorizationFilter>();

            x.Filters.Add(new ResourceFilter("1"));

            x.Filters.Add<ActionFilter>();
            x.Filters.Add<ResultFilter>();
            x.Filters.Add<ExceptionFilter>();
        });

        #endregion

        #region Глобальная регистрация фильтров

        /*services.AddControllers(x =>
        {
            x.Filters.Add<GlobalResultFilter>();
        });*/

        #endregion

        #region DI

        /*services.AddTransient<Action2DIFilter>();
        services.AddControllers(x =>
        {
            x.Filters.Add<Action1DIFilter>();
            
        });*/

        #endregion

        /*services.AddControllers();*/
        
        return services;
    }
}