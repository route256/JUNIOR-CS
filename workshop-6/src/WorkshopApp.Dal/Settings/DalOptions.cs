namespace WorkshopApp.Dal.Settings;

public record DalOptions
{
    public required string PostgresConnectionString { get; init; } = string.Empty;
    
    public required string RedisConnectionString { get; init; } = string.Empty;
}