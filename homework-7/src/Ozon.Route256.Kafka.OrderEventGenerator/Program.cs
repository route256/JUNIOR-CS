using System.Text.Json;
using System.Text.Json.Serialization;

using Ozon.Route256.Kafka.OrderEventGenerator;
using Ozon.Route256.Kafka.OrderEventGenerator.Contracts;
using Ozon.Route256.Kafka.OrderEventGenerator.Kafka;

const string bootstrapServers = "kafka:9092";
const string topicName = "order_events";
const int eventsCount = 100000;
const int timeoutMs = 5 * 60 * 1000;

using var cts = new CancellationTokenSource(timeoutMs);
var publisher = new KafkaPublisher<long, OrderEvent>(
    bootstrapServers,
    topicName,
    keySerializer: null,
    new SystemTextJsonSerializer<OrderEvent>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }));

var generator = new OrderEventGenerator();

var messages = generator
    .GenerateEvents(eventsCount)
    .Select(e => (e.OrderId, e));

await publisher.Publish(messages, cts.Token);
