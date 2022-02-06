using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.Projects.DTO;

namespace Spirebyte.Services.Projects.Application.Projects.Queries;

public class GetProjects : IQuery<IEnumerable<ProjectDto>>
{
    public Guid? OwnerId { get; set; }
}