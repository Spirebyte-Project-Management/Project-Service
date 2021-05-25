using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectGroupDeleted : IEvent
    {
        public Guid ProjectGroupId { get; }

        public ProjectGroupDeleted(Guid projectGroupId)
        {
            ProjectGroupId = projectGroupId;
        }
    }
}
