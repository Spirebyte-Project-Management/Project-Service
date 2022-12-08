using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.DTO;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Queries;

public class GetPermissionScheme : IQuery<PermissionSchemeDto?>
{
    public GetPermissionScheme(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}