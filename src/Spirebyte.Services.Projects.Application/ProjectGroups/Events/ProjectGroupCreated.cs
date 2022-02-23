using System;
using System.Collections.Generic;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Events;

[Contract]
public class ProjectGroupCreated : IEvent
{
    public ProjectGroupCreated(Guid projectGroupId, string projectId, string name, IEnumerable<Guid> userIds)
    {
        Id = projectGroupId;
        ProjectId = projectId;
        Name = name;
        UserIds = userIds;
    }
    public Guid Id { get; }
    public string ProjectId { get; }
    public string Name { get; } 
    public IEnumerable<Guid> UserIds { get; }
}