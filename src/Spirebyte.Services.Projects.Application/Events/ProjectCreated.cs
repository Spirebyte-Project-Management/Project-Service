using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectCreated : IEvent
    {
        public string ProjectId { get; }

        public ProjectCreated(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
