using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("sprints", "sprint_created", "projects.sprint_created")]
public class SprintCreated : IEvent
{
    public SprintCreated(string id, string projectId)
    {
        Id = id;
        ProjectId = projectId;
    }

    public string Id { get; }
    public string ProjectId { get; }
}