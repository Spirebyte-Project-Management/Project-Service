using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Projects.Events;

[Contract]
public class ProjectJoined : IEvent
{
    public ProjectJoined(string projectId, Guid userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }

    public string ProjectId { get; }

    public Guid UserId { get; }
}