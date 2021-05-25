using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Events.External
{
    [Message("issues")]
    public class IssueCreated : IEvent
    {
        public string IssueId { get; }
        public string ProjectId { get; }

        public IssueCreated(string issueId, string projectId)
        {
            IssueId = issueId;
            ProjectId = projectId;
        }
    }
}
