using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Services.Projects.Application.Projects.DTO;

namespace Spirebyte.Services.Projects.Application.Projects.Queries;

public class GetProjects : IQuery<IEnumerable<ProjectDto>>
{
    public Guid? OwnerId { get; set; }
}