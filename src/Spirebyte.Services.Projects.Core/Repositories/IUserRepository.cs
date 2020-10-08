using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Core.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(User user);
    }
}
