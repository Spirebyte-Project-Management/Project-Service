using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Projects.Application.ProjectGroups.DTO;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Queries;

public class GetProjectGroups : IQuery<IEnumerable<ProjectGroupDto>>
{
    public GetProjectGroups()
    {
    }

    public GetProjectGroups(string? projectId)
    {
        ProjectId = projectId;
    }

    public string? ProjectId { get; set; }
}