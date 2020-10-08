using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;
using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Projects.Application.Queries
{
    public class GetProjects : IQuery<IEnumerable<ProjectDto>>
    {
        public Guid? OwnerId { get; set; }
    }
}
