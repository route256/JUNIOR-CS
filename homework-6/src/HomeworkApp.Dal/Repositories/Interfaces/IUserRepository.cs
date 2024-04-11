using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface IUserRepository
{
    Task<long[]> Add(UserEntityV1[] users, CancellationToken token);
    
    Task<UserEntityV1[]> Get(UserGetModel query, CancellationToken token);
}