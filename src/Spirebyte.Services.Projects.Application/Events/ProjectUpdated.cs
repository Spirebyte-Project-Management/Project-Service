using Convey.CQRS.Events;

namespace Spirebyte.Services.Projects.Application.Events
{
    [Contract]
    public class ProjectUpdated : IEvent
    {
        public string ProjectId { get; }

        public ProjectUpdated(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
