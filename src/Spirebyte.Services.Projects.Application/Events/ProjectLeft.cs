using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectLeft : IEvent
    {
        public Guid ProjectId { get; }
        public string Key { get; }

        public Guid UserId { get; }

        public ProjectLeft(Guid projectId, string key, Guid userId)
        {
            ProjectId = projectId;
            Key = key;
            UserId = userId;
        }
    }
}
