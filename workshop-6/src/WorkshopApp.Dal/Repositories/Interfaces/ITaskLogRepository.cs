using WorkshopApp.Dal.Entities;
using WorkshopApp.Dal.Models;

namespace WorkshopApp.Dal.Repositories.Interfaces;

public interface ITaskLogRepository
{
    Task<long[]> Add(TaskLogEntityV1[] tasks, CancellationToken token);
    
    Task<TaskLogEntityV1[]> Get(TaskLogGetModel query, CancellationToken token);
}