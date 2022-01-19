using System;
using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries;

public class GetProjectGroup : IQuery<ProjectGroupDto>
{
    public GetProjectGroup(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}