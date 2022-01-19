using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Events.External;

[Message("issues")]
public class IssueCreated : IEvent
{
    public IssueCreated(string issueId, string projectId, int storyPoints)
    {
        IssueId = issueId;
        ProjectId = projectId;
        StoryPoints = storyPoints;
    }

    public string IssueId { get; }
    public string ProjectId { get; }
    public int StoryPoints { get; }
}