using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Open.Serialization.Json;
using Spirebyte.Framework.Contexts;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Services;

public class PermissionService : IPermissionService
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IContextAccessor _contextAccessor;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<PermissionService> _logger;
    private readonly IUserRepository _userRepository;

    public PermissionService(IProjectRepository projectRepository, ILogger<PermissionService> logger,
        IPermissionSchemeRepository permissionSchemeRepository, IProjectGroupRepository projectGroupRepository,
        IUserRepository userRepository, IJsonSerializer jsonSerializer, IContextAccessor contextAccessor)
    {
        _projectRepository = projectRepository;
        _logger = logger;
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectGroupRepository = projectGroupRepository;
        _userRepository = userRepository;
        _jsonSerializer = jsonSerializer;
        _contextAccessor = contextAccessor;
    }
    
    public async Task<bool> HasPermission(string projectId, string permissionKey)
    {
        if (_contextAccessor.Context?.UserId is null)
        {
            return false;
        }
        return await UserHasPermission(projectId, _contextAccessor.Context.GetUserId(), permissionKey);
    }

    public async Task<bool> HasPermissions(string projectId, params string[] permissionKeys)
    {
        if (_contextAccessor.Context?.UserId is null)
        {
            return false;
        }
        return await UserHasPermissions(projectId, _contextAccessor.Context.GetUserId(), permissionKeys);
    }

    public async Task<bool> UserHasPermission(string projectId, Guid userId, string permissionKey)
    {
        _logger.LogInformation("Checking permission '{permissionKey}' for user '{userId}' on project {projectId}",
            permissionKey, userId, projectId);
        
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
                    var groupIds = _jsonSerializer.Deserialize<Guid[]>(permissionGrant.Value);
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

                    var userIds = _jsonSerializer.Deserialize<Guid[]>(permissionGrant.Value);

                    // Is specifically allowed
                    if (userIds.Contains(userId)) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        _logger.LogInformation("User '{userId}' does not have '{permissionKey}' on project {projectId}",
            userId, permissionKey, projectId);
        return false;
    }

    public async Task<bool> UserHasPermissions(string projectId, Guid userId, params string[] permissionKeys)
    {
        foreach (var permissionKey in permissionKeys)
            if (!await UserHasPermission(projectId, userId, permissionKey))
                return false;

        return true;
    }
}