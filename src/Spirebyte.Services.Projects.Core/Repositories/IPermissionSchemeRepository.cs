using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Core.Repositories
{
    public interface IPermissionSchemeRepository
    {
        Task<PermissionScheme> GetAsync(int permissionSchemeId);
        Task<bool> ExistsAsync(int permissionSchemeId);
        Task AddAsync(PermissionScheme permissionScheme);
        Task UpdateAsync(PermissionScheme permissionScheme);
    }
}
