using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events;

[Contract]
public class ProjectUpdated : IEvent
{
    public ProjectUpdated(string projectId)
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; }
}