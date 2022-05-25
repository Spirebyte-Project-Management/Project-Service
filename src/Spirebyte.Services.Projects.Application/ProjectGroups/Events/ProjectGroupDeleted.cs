﻿using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Core.Entities;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Events;

[Contract]
public class ProjectGroupDeleted : IEvent
{
    public ProjectGroupDeleted(Guid projectGroupId, string projectId, string name, IEnumerable<Guid> userIds)
    {
        Id = projectGroupId;
        ProjectId = projectId;
        Name = name;
        UserIds = userIds;
    }

    public ProjectGroupDeleted(ProjectGroup entity)
    {
        Id = entity.Id;
        ProjectId = entity.ProjectId;
        Name = entity.Name;
        UserIds = entity.UserIds;
    }

    public Guid Id { get; }
    public string ProjectId { get; }
    public string Name { get; }
    public IEnumerable<Guid> UserIds { get; }
}