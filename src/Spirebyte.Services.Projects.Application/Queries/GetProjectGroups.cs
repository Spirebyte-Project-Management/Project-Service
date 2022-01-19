using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries;

public class GetProjectGroups : IQuery<IEnumerable<ProjectGroupDto>>
{
    public GetProjectGroups(string? projectId)
    {
        ProjectId = projectId;
    }

    public string? ProjectId { get; set; }
}