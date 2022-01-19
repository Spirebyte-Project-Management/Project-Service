using System;
using Convey.CQRS.Queries;

namespace Spirebyte.Services.Projects.Application.Queries;

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