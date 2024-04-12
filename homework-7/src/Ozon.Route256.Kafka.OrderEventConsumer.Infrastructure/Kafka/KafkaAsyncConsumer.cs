using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Kafka.OrderEventConsumer.Domain.Common;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.Kafka;

public sealed class KafkaAsyncConsumer<TKey, TValue> : IDisposable
{
    private const int ChannelCapacity = 10; // TODO: IOptions
    private readonly TimeSpan _bufferDelay = TimeSpan.FromSeconds(1); // TODO: IOptions

    private readonly Channel<ConsumeResult<TKey, TValue>> _channel;
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly IHandler<TKey, TValue> _handler;

    private readonly ILogger<KafkaAsyncConsumer<TKey, TValue>> _logger;

    public KafkaAsyncConsumer(
        IHandler<TKey, TValue> handler,
        string bootstrapServers,
        string groupId,
        string topic,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TValue>? valueDeserializer,
        ILogger<KafkaAsyncConsumer<TKey, TValue>> logger)
    {
        var builder = new ConsumerBuilder<TKey, TValue>(
            new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false
            });

        if (keyDeserializer is not null)
        {
            builder.SetKeyDeserializer(keyDeserializer);
        }

        if (valueDeserializer is not null)
        {
            builder.SetValueDeserializer(valueDeserializer);
        }

        _handler = handler;
        _logger = logger;

        _channel = Channel.CreateBounded<ConsumeResult<TKey, TValue>>(
            new BoundedChannelOptions(ChannelCapacity)
            {
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = true,
                FullMode = BoundedChannelFullMode.Wait
            });

        _consumer = builder.Build();
        _consumer.Subscribe(topic);
    }

    public Task Consume(CancellationToken token)
    {
        var handle = HandleCore(token);
        var consume = ConsumeCore(token);

        return Task.WhenAll(handle, consume);
    }

    private async Task HandleCore(CancellationToken token)
    {
        await Task.Yield();

        await foreach (var consumeResults in _channel.Reader
                           .ReadAllAsync(token)
                           .Buffer(ChannelCapacity, _bufferDelay)
                           .WithCancellation(token))
        {
            token.ThrowIfCancellationRequested();

            while (true)
            {
                try
                {
                    await _handler.Handle(consumeResults, token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception occured");
                    continue; // TODO: Polly.Retry
                }

                var partitionLastOffsets = consumeResults
                    .GroupBy(
                        r => r.Partition.Value,
                        (_, f) => f.MaxBy(p => p.Offset.Value));

                foreach (var partitionLastOffset in partitionLastOffsets)
                {
                    _consumer.StoreOffset(partitionLastOffset);
                }

                break; // TODO: Polly.Retry
            }
        }
    }

    private async Task ConsumeCore(CancellationToken token)
    {
        await Task.Yield();

        while (_consumer.Consume(token) is { } result)
        {
            await _channel.Writer.WriteAsync(result, token);
            _logger.LogTrace(
                "{Partition}:{Offset}:WriteToChannel",
                result.Partition.Value,
                result.Offset.Value);
        }

        _channel.Writer.Complete();
    }

    public void Dispose() => _consumer.Close();
}
