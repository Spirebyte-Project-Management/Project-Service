using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories;

public interface ISprintRepository
{
    Task<Sprint> GetAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task AddAsync(Sprint issue);
    Task UpdateAsync(Sprint issue);
}