using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectGroupUpdated : IEvent
    {
        public ProjectGroupUpdated(Guid projectGroupId)
        {
            ProjectGroupId = projectGroupId;
        }

        public Guid ProjectGroupId { get; set; }
    }
}
