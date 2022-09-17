using System;
using Spirebyte.Framework.Shared.Abstractions;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;

public class HasPermission : IQuery<bool>
{
    public HasPermission(string permissionKey, Guid userId, string projectId)
    {
        PermissionKey = permissionKey;
        UserId = userId;
        ProjectId = projectId;
    }

    public string PermissionKey { get; set; }
    public Guid UserId { get; set; }
    public string ProjectId { get; set; }
}