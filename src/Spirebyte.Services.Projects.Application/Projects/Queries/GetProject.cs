using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.Projects.DTO;

namespace Spirebyte.Services.Projects.Application.Projects.Queries;

public class GetProject : IQuery<ProjectDto>
{
    public GetProject(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}