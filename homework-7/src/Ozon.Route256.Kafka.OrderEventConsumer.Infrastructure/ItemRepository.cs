using Ozon.Route256.Kafka.OrderEventConsumer.Domain;

namespace Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure;

public sealed class ItemRepository : IItemRepository
{
    private readonly string _connectionString;

    public ItemRepository(string connectionString) => _connectionString = connectionString;
}
