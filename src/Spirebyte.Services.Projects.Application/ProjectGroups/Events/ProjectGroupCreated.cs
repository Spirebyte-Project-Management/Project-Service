using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Events;

[Message("projects", "project_group_created")]
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