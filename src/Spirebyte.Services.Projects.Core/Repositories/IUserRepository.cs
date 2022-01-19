using System;
using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories;

public interface IUserRepository
{
    Task<User> GetAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(User user);
}