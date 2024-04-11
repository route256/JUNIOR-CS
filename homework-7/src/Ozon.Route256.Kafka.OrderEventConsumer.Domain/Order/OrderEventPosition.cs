using Ozon.Route256.Kafka.OrderEventConsumer.Domain.ValueObjects;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Order;

public sealed record OrderEventPosition(ItemId ItemId, int Quantity, Money Price);
