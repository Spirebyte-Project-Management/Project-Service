using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External.Handlers;

public class SprintDeletedHandler : IEventHandler<SprintDeleted>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;


    public SprintDeletedHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
    }

    public async Task HandleAsync(SprintDeleted @event, CancellationToken cancellationToken = default)
    {
        var sprint = await _sprintRepository.GetAsync(@event.Id);
        if (sprint is null) throw new SprintNotFoundException(@event.Id);

        var project = await _projectRepository.GetAsync(sprint.ProjectId);
        if (project is null) throw new ProjectNotFoundException(sprint.ProjectId);

        project.SprintInsights.SprintRemoved(sprint);
        await _projectRepository.UpdateAsync(project);
    }
}