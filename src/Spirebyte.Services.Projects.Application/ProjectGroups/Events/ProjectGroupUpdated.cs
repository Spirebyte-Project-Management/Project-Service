using System;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Events;

[Contract]
public class ProjectGroupUpdated : IEvent
{
    public ProjectGroupUpdated(Guid projectGroupId)
    {
        ProjectGroupId = projectGroupId;
    }

    public Guid ProjectGroupId { get; set; }
}