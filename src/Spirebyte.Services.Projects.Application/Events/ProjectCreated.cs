using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectCreated : IEvent
    {
        public Guid ProjectId { get; }

        public ProjectCreated(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
