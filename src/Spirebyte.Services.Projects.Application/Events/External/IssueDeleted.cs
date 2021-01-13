using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Events.External
{
    [Message("issues")]
    public class IssueDeleted : IEvent
    {
        public string IssueId { get; }
        public string ProjectId { get; }

        public IssueDeleted(string issueId, string projectId)
        {
            IssueId = issueId;
            ProjectId = projectId;
        }
    }
}
