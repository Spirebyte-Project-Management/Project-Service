using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Services.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermission(string projectId, Guid userId, string permissionKey);
    Task<bool> HasPermissions(string projectId, Guid userId, params string[] permissionKeys);
}