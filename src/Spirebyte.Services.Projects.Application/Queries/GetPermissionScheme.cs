using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;
using System;

namespace Spirebyte.Services.Projects.Application.Queries
{

    public class GetPermissionScheme : IQuery<PermissionSchemeDto>
    {
        public Guid Id { get; set; }

        public GetPermissionScheme(Guid id)
        {
            Id = id;
        }
    }
}
