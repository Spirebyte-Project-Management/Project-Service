using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("issues", "issue_deleted", "projects.issue_deleted")]
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