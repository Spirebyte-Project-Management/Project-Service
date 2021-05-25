using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Events.External.Handlers
{
    public class IssueDeletedHandler : IEventHandler<IssueDeleted>
    {
        private readonly IProjectRepository _projectRepository;


        public IssueDeletedHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task HandleAsync(IssueDeleted @event)
        {
            var project = await _projectRepository.GetAsync(@event.ProjectId);
            if (project is null)
            {
                throw new ProjectNotFoundException(@event.ProjectId);
            }

            project.RemoveIssue();

            await _projectRepository.UpdateAsync(project);
        }
    }
}
