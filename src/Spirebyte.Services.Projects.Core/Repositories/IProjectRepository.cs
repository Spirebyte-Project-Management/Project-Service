using Spirebyte.Services.Projects.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetAsync(Guid projectId);
        Task<Project> GetAsync(string projectKey);
        Task<bool> ExistsWithKeyAsync(string key);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
    }
}
