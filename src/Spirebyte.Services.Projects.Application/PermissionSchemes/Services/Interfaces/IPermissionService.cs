using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermission(string projectId, string permissionKey);
    Task<bool> HasPermissions(string projectId, params string[] permissionKeys);
    
    Task<bool> UserHasPermission(string projectId, Guid userId, string permissionKey);
    Task<bool> UserHasPermissions(string projectId, Guid userId, params string[] permissionKeys);
}