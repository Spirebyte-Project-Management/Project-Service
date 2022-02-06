using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues")]
public class IssueDeleted : IEvent
{
    public IssueDeleted(string issueId, string projectId)
    {
        IssueId = issueId;
        ProjectId = projectId;
    }

    public string IssueId { get; }
    public string ProjectId { get; }
}