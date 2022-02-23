using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues")]
public class IssueCreated : IEvent
{
    public IssueCreated(string id, string projectId, int storyPoints)
    {
        Id = id;
        ProjectId = projectId;
        StoryPoints = storyPoints;
    }

    public string Id { get; }
    public string ProjectId { get; }
    public int StoryPoints { get; }
}