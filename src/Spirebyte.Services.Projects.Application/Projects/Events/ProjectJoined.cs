using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Contract]
public class ProjectJoined : IEvent
{
    public ProjectJoined(string id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public string Id { get; }

    public Guid UserId { get; }
}