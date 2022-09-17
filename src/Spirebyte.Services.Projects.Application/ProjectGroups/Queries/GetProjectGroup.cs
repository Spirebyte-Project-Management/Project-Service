using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Queries;

public class GetProjectGroup : IQuery<ProjectGroupDto>
{
    public GetProjectGroup(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}