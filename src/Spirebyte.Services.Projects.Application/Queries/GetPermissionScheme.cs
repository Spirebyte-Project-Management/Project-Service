using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries
{

    public class GetPermissionScheme : IQuery<PermissionSchemeDto>
    {
        public int Id { get; set; }

        public GetPermissionScheme(int id)
        {
            Id = id;
        }
    }
}
