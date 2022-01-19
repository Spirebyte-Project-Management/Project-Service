using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events;

[Contract]
public class ProjectCreated : IEvent
{
    public ProjectCreated(string projectId)
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; }
}