using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkshopApp.Dal.Infrastructure;
using WorkshopApp.Dal.Repositories;
using WorkshopApp.Dal.Repositories.Interfaces;
using WorkshopApp.Dal.Settings;

namespace WorkshopApp.Dal.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalRepositories(
        this IServiceCollection services)
    {
        AddPostgresRepositories(services);
        AddRedisRepositories(services);
        
        return services;
    }

    private static void AddRedisRepositories(IServiceCollection services)
    {
        services.AddScoped<ITakenTaskRepository, TakenTaskRepository>();
        services.AddScoped<IUserScheduleRepository, UserScheduleRepository>();
    }

    private static void AddPostgresRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskLogRepository, TaskLogRepository>();
    }

    public static IServiceCollection AddDalInfrastructure(
        this IServiceCollection services, 
        IConfigurationRoot config)
    {
        //read config
        services.Configure<DalOptions>(config.GetSection(nameof(DalOptions)));

        //configure postrges types
        Postgres.MapCompositeTypes();
        
        //add migrations
        Postgres.AddMigrations(services);
        
        return services;

    }
}