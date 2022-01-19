using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Projects.Application.Commands;

[Contract]
public class CreateProjectGroup : ICommand
{
    public CreateProjectGroup(Guid projectGroupId, string projectId, string name, IEnumerable<Guid> userIds)
    {
        ProjectGroupId = projectGroupId;
        ProjectId = projectId;
        Name = name;
        UserIds = userIds;
    }

    public Guid ProjectGroupId { get; set; }
    public string ProjectId { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> UserIds { get; set; }
}