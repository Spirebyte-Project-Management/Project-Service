using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectJoined : IEvent
    {
        public string ProjectId { get; }

        public Guid UserId { get; }

        public ProjectJoined(string projectId, Guid userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }
    }
}
