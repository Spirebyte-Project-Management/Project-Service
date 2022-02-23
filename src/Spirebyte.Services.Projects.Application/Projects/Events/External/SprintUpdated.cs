using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External;

[Message("sprints")]
public class SprintUpdated : IEvent
{
    public SprintUpdated(string id, DateTime startedAt, DateTime endedAt)
    {
        Id = id;
        StartedAt = startedAt;
        EndedAt = endedAt;
    }

    public string Id { get; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }

    public SprintStatus Status => GetStatus();

    private SprintStatus GetStatus()
    {
        if (StartedAt == DateTime.MinValue)
            return SprintStatus.PLANNED;
        if (StartedAt != DateTime.MinValue && EndedAt == DateTime.MinValue)
            return SprintStatus.ACTIVE;
        if (StartedAt != DateTime.MinValue && EndedAt != DateTime.MinValue)
            return SprintStatus.COMPLETED;
        return SprintStatus.PLANNED;
    }
}