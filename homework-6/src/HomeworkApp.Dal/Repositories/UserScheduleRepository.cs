using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;

namespace HomeworkApp.Dal.Repositories;

public class UserScheduleRepository : RedisRepository, IUserScheduleRepository
{
    protected override string KeyPrefix => "user_schedule";

    public UserScheduleRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value) { }

    
    public async Task Init(UserScheduleModel model, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(
            model.UserId, 
            "year", model.Year, 
            "month", model.Month);

        for (var index = 0; index < model.ScheduleMap.Length; index++)
        {
            await connection.StringSetBitAsync(key,
                index,
                model.ScheduleMap[index]);
        }
    }
    
    public async Task<bool> IsWorkDay(
        long userId, 
        DateOnly date,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(
            userId, 
            "year", date.Year, 
            "month", date.Month);
        
        return await connection.StringGetBitAsync(key, date.Day - 1);
    }
    
    public async Task TakeDayOff(
        long userId, 
        DateOnly date,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(
            userId, 
            "year", date.Year, 
            "month", date.Month);
        
        await connection.StringSetBitAsync(key, date.Day - 1, false);
    }
    
    public async Task TakeExtraDay(
        long userId, 
        DateOnly date,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var connection = await GetConnection();

        var key = GetKey(
            userId, 
            "year", date.Year, 
            "month", date.Month);
        
        await connection.StringSetBitAsync(key, date.Day - 1, true);
    }
}