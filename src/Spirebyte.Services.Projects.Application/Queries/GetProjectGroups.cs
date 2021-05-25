using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class GetProjectGroups : IQuery<IEnumerable<ProjectGroupDto>>
    {
        public string? ProjectId { get; set; }

        public GetProjectGroups(string? projectId)
        {
            ProjectId = projectId;
        }
    }
}
