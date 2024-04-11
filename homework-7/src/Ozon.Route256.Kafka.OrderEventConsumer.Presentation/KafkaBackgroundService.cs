using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public class KafkaBackgroundService : BackgroundService
{
    private readonly KafkaAsyncConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaBackgroundService> _logger;

    public KafkaBackgroundService(IServiceProvider serviceProvider, ILogger<KafkaBackgroundService> logger)
    {
        // TODO: IOptions
        // TODO: KafkaServiceExtensions: services.AddKafkaHandler<TKey, TValue, THandler<TKey, TValue>>(serializers, topic, groupId);
        _logger = logger;
        var handler = serviceProvider.GetRequiredService<ItemHandler>();
        _consumer = new KafkaAsyncConsumer<Ignore, string>(
            handler,
            "kafka:9092",
            "group_id",
            "order_events",
            null,
            null,
            serviceProvider.GetRequiredService<ILogger<KafkaAsyncConsumer<Ignore, string>>>());
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Dispose();

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _consumer.Consume(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occured");
        }
    }
}
