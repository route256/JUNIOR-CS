using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HomeworkApp.Dal.Repositories;

public class TakenTaskRepository : RedisRepository, ITakenTaskRepository
{
    protected override TimeSpan KeyTtl => TimeSpan.FromMinutes(5);
    
    protected override string KeyPrefix => "taken_tasks";

    public TakenTaskRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value) { }

    public async Task Add(TakenTaskModel model, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(model.TaskId);
        await connection.HashSetAsync(key, new HashEntry[]
        {
            new ("task_id", model.TaskId),
            new ("title", model.Title),
            new ("assigned_to_user_id", model.AssignedToUserId),
            new ("assigned_to_email", model.AssignedToEmail),
            new ("assigned_at", model.AssignedAt.ToUnixTimeMilliseconds()),
        });

        await connection.KeyExpireAsync(key, KeyTtl);
    }
    
    public async Task<TakenTaskModel?> Get(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(taskId);
        var fields = await connection.HashGetAllAsync(key);

        if (!fields.Any())
        {
            return null;
        }

        var result = new TakenTaskModel(); 
        foreach (var field in fields)
        {
            if (!field.Value.HasValue)
            {
                continue;
            }

            field.Value.TryParse(out long longValue);
            var strValue = field.Value.ToString();
            
            result = field.Name.ToString() switch
            {
                "task_id" => result with {TaskId = longValue},
                "title" => result with {Title = strValue},
                "assigned_to_user_id" => result with {AssignedToUserId = longValue},
                "assigned_to_email" => result with {AssignedToEmail = strValue},
                "assigned_at" => result with{AssignedAt = DateTimeOffset.FromUnixTimeMilliseconds(longValue)},
                _ => result
            };
        }

        return result;
    }

    public async Task<DateTimeOffset?> GetTakenAt(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(taskId);
        var fields = await connection.HashGetAsync(key, new []
        {
            new RedisValue("assigned_at")
        });

        if (!fields.Any())
        {
            return null;
        }

        fields.First().TryParse(out long longValue);

        return DateTimeOffset.FromUnixTimeMilliseconds(longValue);
    }
    
    public async Task<DateTime?> GetExpireIfExists(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();
        var key = GetKey(taskId);
        
        return await connection.KeyExpireTimeAsync(key);
    }

    public async Task Delete(long taskId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(taskId);
        await connection.KeyDeleteAsync(key);
    }
    
}