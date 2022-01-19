using System;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries;

public class GetPermissionScheme : IQuery<PermissionSchemeDto>
{
    public GetPermissionScheme(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}