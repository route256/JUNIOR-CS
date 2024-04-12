using Dapper;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;

namespace HomeworkApp.Dal.Repositories;

public class UserRepository : PgRepository, IUserRepository
{
    public UserRepository(
        IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task<long[]> Add(UserEntityV1[] users, CancellationToken token)
    {
        const string sqlQuery = @"
insert into users (email, created_at, blocked_at)
select email, created_at, blocked_at
  from UNNEST(@Users)
returning id;
";

        await using var connection = await GetConnection();
        var ids = await connection.QueryAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Users = users
                },
                cancellationToken: token));
        
        return ids
            .ToArray();
    }

    public async Task<UserEntityV1[]> Get(UserGetModel query, CancellationToken token)
    {
        var baseSql = @"
select id
     , email
     , created_at
     , blocked_at
  from users
";

        var conditions = new List<string>();
        var @params = new DynamicParameters();

        if (query.UserIds.Any())
        {
            conditions.Add($"id = ANY(@UserIds)");
            @params.Add($"UserIds", query.UserIds);
        }
        
        var cmd = new CommandDefinition(
            baseSql + $" WHERE {string.Join(" AND ", conditions)} ",
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);
        
        await using var connection = await GetConnection();
        return (await connection.QueryAsync<UserEntityV1>(cmd))
            .ToArray();
    }
}