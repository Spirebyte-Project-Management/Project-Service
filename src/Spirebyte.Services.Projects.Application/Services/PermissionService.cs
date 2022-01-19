using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public PermissionService(IProjectRepository projectRepository,
        IPermissionSchemeRepository permissionSchemeRepository, IProjectGroupRepository projectGroupRepository,
        IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectGroupRepository = projectGroupRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> HasPermission(string projectId, Guid userId, string permissionKey)
    {
        if (!await _projectRepository.ExistsAsync(projectId)) throw new ProjectNotFoundException(projectId);

        if (!await _userRepository.ExistsAsync(userId)) throw new UserNotFoundException(userId);

        var project = await _projectRepository.GetAsync(projectId);
        var permissionScheme = await _permissionSchemeRepository.GetAsync(project.PermissionSchemeId);

        var permission = permissionScheme.Permissions.FirstOrDefault(ps => ps.Key == permissionKey);

        if (permission == null) throw new PermissionNotFoundException(permissionKey);

        foreach (var permissionGrant in permission.Grants)
            switch (permissionGrant.Type)
            {
                case GrantTypes.Anyone:
                    return true;
                    break;
                case GrantTypes.ProjectGroup:
                    // Is user part of project group
                    var groupIds = JsonConvert.DeserializeObject<Guid[]>(permissionGrant.Value);
                    foreach (var groupId in groupIds)
                    {
                        var group = await _projectGroupRepository.GetAsync(groupId);
                        if (group.UserIds.Contains(userId)) return true;
                    }

                    break;
                case GrantTypes.ProjectLead:
                    // If user is project leader
                    if (project.OwnerUserId == userId) return true;
                    break;
                case GrantTypes.ProjectUser:
                    if (string.IsNullOrWhiteSpace(permissionGrant.Value))
                    {
                        // Is project owner
                        if (project.OwnerUserId == userId) return true;

                        // Is project user
                        if (project.ProjectUserIds.Contains(userId)) return true;

                        continue;
                    }

                    var userIds = JsonConvert.DeserializeObject<Guid[]>(permissionGrant.Value);

                    // Is specifically allowed
                    if (userIds.Contains(userId)) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        return false;
    }

    public async Task<bool> HasPermissions(string projectId, Guid userId, params string[] permissionKeys)
    {
        foreach (var permissionKey in permissionKeys)
            if (!await HasPermission(projectId, userId, permissionKey))
                return false;

        return true;
    }
}