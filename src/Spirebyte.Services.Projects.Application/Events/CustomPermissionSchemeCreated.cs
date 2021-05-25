using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class CustomPermissionSchemeCreated : IEvent
    {
        public string ProjectId { get; }
        public Guid PermissionSchemeId { get; }

        public CustomPermissionSchemeCreated(string projectId, Guid permissionSchemeId)
        {
            ProjectId = projectId;
            PermissionSchemeId = permissionSchemeId;
        }
    }
}
