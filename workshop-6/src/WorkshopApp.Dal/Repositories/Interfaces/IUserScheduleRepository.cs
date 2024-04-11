using WorkshopApp.Dal.Models;

namespace WorkshopApp.Dal.Repositories.Interfaces;

public interface IUserScheduleRepository
{
    Task Init(UserScheduleModel model, CancellationToken token);

    Task<bool> IsWorkDay(long userId, DateOnly date, CancellationToken token);

    Task TakeDayOff(long userId, DateOnly date, CancellationToken token);

    Task TakeExtraDay(long userId, DateOnly date, CancellationToken token);
}