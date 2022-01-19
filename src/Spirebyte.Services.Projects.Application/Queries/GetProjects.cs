using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries;

public class GetProjects : IQuery<IEnumerable<ProjectDto>>
{
    public Guid? OwnerId { get; set; }
}