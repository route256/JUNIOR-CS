using System.Text.Json.Serialization;

namespace Ozon.Route256.Kafka.OrderEventGenerator.Contracts;

public sealed class OrderEvent
{
    [JsonPropertyName("order_id")]
    public long OrderId { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("warehouse_id")]
    public long WarehouseId { get; set; }

    [JsonPropertyName("status")]
    public OrderStatus Status { get; set; }

    [JsonPropertyName("moment")]
    public DateTime Moment { get; set; }

    [JsonPropertyName("positions")]
    public OrderEventPosition[] Positions { get; set; } = null!;

    public sealed class OrderEventPosition
    {
        [JsonPropertyName("item_id")]
        public long ItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public Money Price { get; set; } = null!;
    }

    public sealed class Money
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = null!;

        [JsonPropertyName("units")]
        public long Units { get; set; }

        [JsonPropertyName("nanos")]
        public int Nanos { get; set; }
    }

    public enum OrderStatus
    {
        Created,
        Delivered,
        Canceled
    }
}
