using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Core.Entities;

public class Sprint
{
    public Sprint(string id, string projectId, SprintStatus status)
    {
        Id = id;
        ProjectId = projectId;
        Status = status;
    }

    public string Id { get; }
    public string ProjectId { get; }
    public SprintStatus Status { get; set; }
}