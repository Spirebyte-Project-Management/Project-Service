using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues", "issue_created", "projects.issue_created")]
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