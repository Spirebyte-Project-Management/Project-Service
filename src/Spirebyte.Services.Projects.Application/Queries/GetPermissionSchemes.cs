using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class GetPermissionSchemes : IQuery<IEnumerable<PermissionSchemeDto>>
    {
        public string? ProjectId { get; set; }

        public GetPermissionSchemes(string? projectId)
        {
            ProjectId = projectId;
        }
    }
}
