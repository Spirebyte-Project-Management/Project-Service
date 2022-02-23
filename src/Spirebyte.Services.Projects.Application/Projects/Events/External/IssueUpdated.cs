using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues")]
public class IssueUpdated : IEvent
{
    public IssueUpdated(string id, int storyPoints, IssueStatus status)
    {
        Id = id;
        StoryPoints = storyPoints;
        Status = status;
    }

    public string Id { get; }
    public int StoryPoints { get; }

    public IssueStatus Status { get; }
}