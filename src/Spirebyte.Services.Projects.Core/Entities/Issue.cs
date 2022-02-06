using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Core.Entities;

public class Issue
{
    public Issue(string id, string projectId, IssueStatus status)
    {
        Id = id;
        ProjectId = projectId;
        Status = status;
    }

    public string Id { get; }
    public string ProjectId { get; }
    public IssueStatus Status { get; set; }
}