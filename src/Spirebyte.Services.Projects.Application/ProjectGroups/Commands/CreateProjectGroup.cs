using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands;

[Contract]
public record CreateProjectGroup
    (Guid ProjectGroupId, string ProjectId, string Name, IEnumerable<Guid> UserIds) : ICommand
{
    public Guid ProjectGroupId = Guid.NewGuid();
}