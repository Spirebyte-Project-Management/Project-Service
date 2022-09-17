using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("sprints", "sprint_created", "projects.sprint_created")]
public class SprintDeleted : IEvent
{
    public SprintDeleted(string id)
    {
        Id = id;
    }

    public string Id { get; }
}