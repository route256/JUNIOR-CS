using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLogging();

        var connectionString = _configuration["ConnectionString"]!;
        services
            .AddFluentMigrator(
                connectionString,
                typeof(SqlMigration).Assembly);

        services.AddSingleton<IItemRepository, ItemRepository>(_ => new ItemRepository(connectionString));
        services.AddSingleton<ItemHandler>();
        services.AddHostedService<KafkaBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
