using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectGroupCreated : IEvent
    {
        public Guid ProjectGroupId { get; }

        public ProjectGroupCreated(Guid projectGroupId)
        {
            ProjectGroupId = projectGroupId;
        }
    }
}
