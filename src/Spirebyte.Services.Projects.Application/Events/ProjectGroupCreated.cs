using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

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
