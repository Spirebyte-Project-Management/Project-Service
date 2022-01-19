using Convey.CQRS.Queries;
using Spirebyte.Services.Projects.Application.DTO;

namespace Spirebyte.Services.Projects.Application.Queries;

public class GetProject : IQuery<ProjectDto>
{
    public GetProject(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}