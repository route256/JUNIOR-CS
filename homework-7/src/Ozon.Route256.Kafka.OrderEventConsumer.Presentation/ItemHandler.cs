using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Presentation;

public class ItemHandler : IHandler<Ignore, string>
{
    private readonly ILogger<ItemHandler> _logger;
    private readonly Random _random = new();

    public ItemHandler(ILogger<ItemHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, string>> messages, CancellationToken token)
    {
        await Task.Delay(_random.Next(300), token);
        _logger.LogInformation("Handled {Count} messages", messages.Count);
    }
}
