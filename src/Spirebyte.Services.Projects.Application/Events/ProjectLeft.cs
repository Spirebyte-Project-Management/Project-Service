using Convey.CQRS.Events;
using System;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectLeft : IEvent
    {
        public string ProjectId { get; }

        public Guid UserId { get; }

        public ProjectLeft(string projectId, Guid userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }
    }
}
