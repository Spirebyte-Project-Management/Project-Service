using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.DTO;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;

public class GetPermissionSchemes : IQuery<IEnumerable<PermissionSchemeDto>>
{
    public GetPermissionSchemes(string? projectId)
    {
        ProjectId = projectId;
    }

    public string? ProjectId { get; set; }
}