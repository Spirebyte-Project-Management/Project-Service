using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class GetProjectGroup : IQuery<ProjectGroupDto>
    {
        public Guid Id { get; set; }

        public GetProjectGroup(Guid id)
        {
            Id = id;
        }
    }
}
