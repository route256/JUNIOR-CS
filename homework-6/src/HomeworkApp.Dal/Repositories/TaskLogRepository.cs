using Dapper;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;

namespace HomeworkApp.Dal.Repositories;

public class TaskLogRepository : PgRepository, ITaskLogRepository
{
    public TaskLogRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task<long[]> Add(TaskLogEntityV1[] logs, CancellationToken token)
    {
        const string sqlQuery = @"
insert into task_logs (task_id, parent_task_id, number, title, description, status, assigned_to_user_id, user_id, at)  
select task_id, parent_task_id, number, title, description, status, assigned_to_user_id, user_id, at
  from UNNEST(@Logs)
returning id;
";

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Logs = logs
                },
                cancellationToken: token));
        
        return ids
            .ToArray();
    }

    public async Task<TaskLogEntityV1[]> Get(TaskLogGetModel query, CancellationToken token)
    {
        var baseSql = @"
select id
     , task_id
     , parent_task_id
     , number
     , title
     , description
     , status
     , assigned_to_user_id
     , user_id, at
  from task_logs
";
        
        var conditions = new List<string>();
        var @params = new DynamicParameters();

        if (query.TaskIds.Any())
        {
            conditions.Add($"task_id = ANY(@TaskIds)");
            @params.Add($"TaskIds", query.TaskIds);
        }
        
        var cmd = new CommandDefinition(
            baseSql + $" WHERE {string.Join(" AND ", conditions)} ",
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);
        
        await using var connection = await GetConnection();
        return (await connection.QueryAsync<TaskLogEntityV1>(cmd))
            .ToArray();
    }
}