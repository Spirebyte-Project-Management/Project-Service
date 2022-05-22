using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Shared.Changes;
using Spirebyte.Shared.Changes.ValueObjects;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Events;

[Contract]
public class ProjectGroupUpdated : IEvent
{
    public ProjectGroupUpdated(Guid projectGroupId, string projectId, string name, IEnumerable<Guid> userIds)
    {
        Id = projectGroupId;
        ProjectId = projectId;
        Name = name;
        UserIds = userIds;
    }
    
    public ProjectGroupUpdated(ProjectGroup entity, ProjectGroup oldProject)
    {
        Id = entity.Id;
        ProjectId = entity.ProjectId;
        Name = entity.Name;
        UserIds = entity.UserIds;

        Changes = ChangedFieldsHelper.GetChanges(oldProject, entity);
    }

    public Guid Id { get; }
    public string ProjectId { get; }
    public string Name { get; } 
    public IEnumerable<Guid> UserIds { get; }
    
    public Change[] Changes { get; set; }

}