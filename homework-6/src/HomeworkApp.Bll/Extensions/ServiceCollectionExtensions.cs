using HomeworkApp.Bll.Services;
using HomeworkApp.Bll.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HomeworkApp.Bll.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllServices(
        this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        
        return services;
    }
}