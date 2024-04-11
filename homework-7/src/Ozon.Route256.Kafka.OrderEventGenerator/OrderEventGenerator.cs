using AutoFixture;

using Ozon.Route256.Kafka.OrderEventGenerator.Contracts;

namespace Ozon.Route256.Kafka.OrderEventGenerator;

internal sealed class OrderEventGenerator
{
    public IEnumerable<OrderEvent> GenerateEvents(int eventsCount)
    {
        var rnd = new Random();
        var sentOrders = new Dictionary<long, OrderEvent>(eventsCount);

        var sellerIds = Enumerable
            .Range(start: 0, count: 20_000)
            .Select(_ => (long)rnd.Next(minValue: 100_000, maxValue: 300_000) * 1_000_000)
            .ToArray();

        var currencies = new[] { "RUB", "KZT" };
        var items = Enumerable
            .Range(start: 0, count: 300_000)
            .Select(_ => (long)rnd.Next(minValue: 1, maxValue: 999_999))
            .Select(
                shortId => (
                    Id: shortId + sellerIds[rnd.Next(sellerIds.Length)],
                    Currency: currencies[rnd.Next(currencies.Length)],
                    PriceUnits: rnd.Next(minValue: 300, maxValue: 10_000),
                    PriceNanos: rnd.Next(minValue: 0, maxValue: 99) * 10_000_000))
            .ToArray();

        var fixture = new Fixture();
        fixture
            .Customize<OrderEvent.OrderEventPosition>(
                transform => transform
                    .FromFactory(
                        () =>
                        {
                            var item = items[rnd.Next(items.Length)];

                            return new OrderEvent.OrderEventPosition
                            {
                                ItemId = item.Id,
                                Price = new OrderEvent.Money
                                {
                                    Currency = item.Currency,
                                    Units = item.PriceUnits,
                                    Nanos = item.PriceNanos
                                },
                                Quantity = rnd.Next(minValue: 1, maxValue: 20)
                            };
                        })
                    .OmitAutoProperties());
        var events = 0;

        fixture
            .Customize<OrderEvent>(
                t => t
                    .With(d => d.Moment, DateTime.UtcNow - TimeSpan.FromSeconds(eventsCount - events))
                    .With(d => d.Status, OrderEvent.OrderStatus.Created));

        while (events < eventsCount)
        {
            var orderId = rnd.Next(minValue: 1, maxValue: (int)(eventsCount * 1.5));

            if (sentOrders.TryGetValue(orderId, out var sentOrderEvent))
            {
                if (sentOrderEvent.Status != OrderEvent.OrderStatus.Created)
                {
                    continue;
                }

                sentOrderEvent.Status = rnd.NextSingle() < 0.7
                    ? OrderEvent.OrderStatus.Delivered
                    : OrderEvent.OrderStatus.Canceled;

                sentOrderEvent.Moment = DateTime.UtcNow - TimeSpan.FromSeconds(eventsCount - events);

                yield return sentOrderEvent;
            }
            else
            {
                var orderEvent = fixture.Create<OrderEvent>();
                orderEvent.OrderId = orderId;
                sentOrders.Add(orderEvent.OrderId, orderEvent);

                yield return orderEvent;
            }
            ++events;
        }
    }
}
