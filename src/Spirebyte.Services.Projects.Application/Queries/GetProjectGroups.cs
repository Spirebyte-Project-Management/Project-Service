using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

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
