using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events;

[Contract]
public class ProjectLeft : IEvent
{
    public ProjectLeft(string projectId, Guid userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }

    public string ProjectId { get; }

    public Guid UserId { get; }
}