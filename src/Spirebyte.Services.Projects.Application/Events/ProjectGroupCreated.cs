using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events;

[Contract]
public class ProjectGroupCreated : IEvent
{
    public ProjectGroupCreated(Guid projectGroupId)
    {
        ProjectGroupId = projectGroupId;
    }

    public Guid ProjectGroupId { get; }
}