using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface ITakenTaskRepository : IRedisRepository
{
    Task Add(TakenTaskModel model, CancellationToken token);

    Task<TakenTaskModel?> Get(long taskId, CancellationToken token);

    Task<DateTimeOffset?> GetTakenAt(long taskId, CancellationToken token);

    Task<DateTime?> GetExpireIfExists(long taskId, CancellationToken token);

    Task Delete(long taskId, CancellationToken token);
}