using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Events;

[Contract]
public class ProjectGroupDeleted : IEvent
{
    public ProjectGroupDeleted(Guid projectGroupId)
    {
        ProjectGroupId = projectGroupId;
    }

    public Guid ProjectGroupId { get; }
}