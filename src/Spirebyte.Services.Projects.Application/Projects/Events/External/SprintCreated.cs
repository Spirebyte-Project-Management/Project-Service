using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("sprints")]
public class SprintCreated : IEvent
{
    public SprintCreated(string sprintId, string projectId)
    {
        SprintId = sprintId;
        ProjectId = projectId;
    }

    public string SprintId { get; }
    public string ProjectId { get; }
}