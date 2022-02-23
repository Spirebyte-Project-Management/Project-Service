using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Contract]
public class ProjectLeft : IEvent
{
    public ProjectLeft(string id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public string Id { get; }

    public Guid UserId { get; }
}