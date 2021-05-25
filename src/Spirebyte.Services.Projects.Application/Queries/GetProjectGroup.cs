using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;
using System;

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
