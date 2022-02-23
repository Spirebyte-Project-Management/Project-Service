using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues")]
public class IssueDeleted : IEvent
{
    public IssueDeleted(string id, string projectId)
    {
        Id = id;
        ProjectId = projectId;
    }

    public string Id { get; }
    public string ProjectId { get; }
}