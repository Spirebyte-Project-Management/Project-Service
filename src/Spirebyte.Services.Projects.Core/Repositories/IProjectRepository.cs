using Spirebyte.Services.Projects.Core.Entities;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetAsync(string projectId);
        Task<bool> ExistsAsync(string projectId);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(string projectId);
    }
}
