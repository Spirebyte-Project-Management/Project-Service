using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands;

[Message("projects", "create_project_group", "projects.create_project_group")]
public record CreateProjectGroup
    (string ProjectId, string Name, IEnumerable<Guid> UserIds) : ICommand
{
    public Guid ProjectGroupId = Guid.NewGuid();
}