using WorkshopApp.Bll.Models;

namespace WorkshopApp.Bll.Services.Interfaces;

public interface ITaskService
{
    Task<long> CreateTask(CreateTaskModel model, CancellationToken token);

    Task<GetTaskModel?> GetTask(long taskId, CancellationToken token);

    Task AssignTask(Bll.Models.AssignTaskModel model, CancellationToken token);
}