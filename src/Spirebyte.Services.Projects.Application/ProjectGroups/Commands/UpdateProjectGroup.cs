using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands;

[Message("projects", "update_project_group", "projects.update_project_group")]
public class UpdateProjectGroup : ICommand
{
    public UpdateProjectGroup(Guid id, string projectId, string name, IEnumerable<Guid> userIds)
    {
        Id = id;
        ProjectId = projectId;
        Name = name;
        UserIds = userIds;
    }

    public Guid Id { get; set; }
    public string ProjectId { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> UserIds { get; set; }
}