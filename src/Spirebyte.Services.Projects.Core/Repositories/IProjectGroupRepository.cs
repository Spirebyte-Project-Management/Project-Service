using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories
{
    public interface IProjectGroupRepository
    {
        Task<ProjectGroup> GetAsync(Guid projectRoleId);
        Task<bool> ExistsAsync(Guid projectRoleId);
        Task<bool> ExistsWithNameAsync(string name);
        Task AddAsync(ProjectGroup projectGroup);
        Task UpdateAsync(ProjectGroup projectGroup);
    }
}
