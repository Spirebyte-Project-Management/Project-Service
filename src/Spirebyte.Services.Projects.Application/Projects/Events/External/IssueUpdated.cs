using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues")]
public class IssueUpdated : IEvent
{
    public IssueUpdated(string issueId, int storyPoints, IssueStatus status)
    {
        IssueId = issueId;
        StoryPoints = storyPoints;
        Status = status;
    }

    public string IssueId { get; }
    public int StoryPoints { get; }

    public IssueStatus Status { get; }
}