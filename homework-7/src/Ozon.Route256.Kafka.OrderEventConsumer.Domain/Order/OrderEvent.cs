using System;

using Ozon.Route256.Kafka.OrderEventConsumer.Domain.ValueObjects;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Domain.Order;

public sealed record OrderEvent(
    OrderId OrderId,
    UserId UserId,
    WarehouseId WarehouseId,
    Status Status,
    DateTime Moment,
    OrderEventPosition[] Positions);
