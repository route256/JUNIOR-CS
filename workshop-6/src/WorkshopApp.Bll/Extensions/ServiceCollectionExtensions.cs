using Microsoft.Extensions.DependencyInjection;
using WorkshopApp.Bll.Services;
using WorkshopApp.Bll.Services.Interfaces;

namespace WorkshopApp.Bll.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllServices(
        this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        
        return services;
    }
}