using WorkshopApp.Dal.Entities;
using WorkshopApp.Dal.Models;

namespace WorkshopApp.Dal.Repositories.Interfaces;

public interface IUserRepository
{
    Task<long[]> Add(UserEntityV1[] users, CancellationToken token);
    
    Task<UserEntityV1[]> Get(UserGetModel query, CancellationToken token);
}