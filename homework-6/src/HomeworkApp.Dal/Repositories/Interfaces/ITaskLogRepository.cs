using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface ITaskLogRepository
{
    Task<long[]> Add(TaskLogEntityV1[] tasks, CancellationToken token);
    
    Task<TaskLogEntityV1[]> Get(TaskLogGetModel query, CancellationToken token);
}