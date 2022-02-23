using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External.Handlers;

public class SprintUpdatedHandler : IEventHandler<SprintUpdated>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;


    public SprintUpdatedHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
    }

    public async Task HandleAsync(SprintUpdated @event, CancellationToken cancellationToken = default)
    {
        var sprint = await _sprintRepository.GetAsync(@event.Id);
        if (sprint is null) throw new SprintNotFoundException(@event.Id);

        var project = await _projectRepository.GetAsync(sprint.ProjectId);
        if (project is null) throw new ProjectNotFoundException(sprint.ProjectId);

        var updatedSprint = new Sprint(sprint.Id, sprint.ProjectId, @event.Status);
        await _sprintRepository.UpdateAsync(updatedSprint);

        project.SprintInsights.SprintUpdated(updatedSprint, sprint);
        await _projectRepository.UpdateAsync(project);
    }
}