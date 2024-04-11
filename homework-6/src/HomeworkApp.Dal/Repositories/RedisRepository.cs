using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using StackExchange.Redis;

namespace HomeworkApp.Dal.Repositories;

public abstract class RedisRepository : IRedisRepository
{
    // we must reuse it as it possible
    private static ConnectionMultiplexer? _connection;
    
    private readonly DalOptions _dalSettings;

    protected RedisRepository(DalOptions dalSettings)
    {
        _dalSettings = dalSettings;
    }

    protected abstract string KeyPrefix { get; }
    
    protected virtual TimeSpan KeyTtl => TimeSpan.MaxValue;
    
    protected async Task<IDatabase> GetConnection()
    {
        _connection ??= await ConnectionMultiplexer.ConnectAsync(_dalSettings.RedisConnectionString);
        
        return _connection.GetDatabase();
    }
    
    protected RedisKey GetKey(params object[] identifiers)
        => new ($"{KeyPrefix}:{string.Join(':', identifiers)}");
}