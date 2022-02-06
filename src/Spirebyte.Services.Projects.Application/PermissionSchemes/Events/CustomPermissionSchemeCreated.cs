using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Events;

[Contract]
public class CustomPermissionSchemeCreated : IEvent
{
    public CustomPermissionSchemeCreated(string projectId, Guid permissionSchemeId)
    {
        ProjectId = projectId;
        PermissionSchemeId = permissionSchemeId;
    }

    public string ProjectId { get; }
    public Guid PermissionSchemeId { get; }
}