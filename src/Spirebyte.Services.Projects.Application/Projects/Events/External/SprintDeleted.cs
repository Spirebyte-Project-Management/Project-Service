using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("sprints")]
public class SprintDeleted : IEvent
{
    public SprintDeleted(string id)
    {
        Id = id;
    }

    public string Id { get; }
}