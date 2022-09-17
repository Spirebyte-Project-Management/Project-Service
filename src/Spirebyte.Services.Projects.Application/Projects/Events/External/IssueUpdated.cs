using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues", "issue_updated", "projects.issue_updated")]
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