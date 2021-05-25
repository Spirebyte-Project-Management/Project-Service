using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectPermissionSchemeUpdated : IEvent
    {
        public ProjectPermissionSchemeUpdated(Guid projectPermissionSchemeId)
        {
            ProjectPermissionSchemeId = projectPermissionSchemeId;
        }

        public Guid ProjectPermissionSchemeId { get; set; }
    }
}
