using WorkshopApp.Dal.Entities;
using WorkshopApp.Dal.Models;

namespace WorkshopApp.Dal.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<long[]> Add(TaskEntityV1[] tasks, CancellationToken token);
    
    Task<TaskEntityV1[]> Get(TaskGetModel query, CancellationToken token);

    Task Assign(AssignTaskModel model, CancellationToken token);
}