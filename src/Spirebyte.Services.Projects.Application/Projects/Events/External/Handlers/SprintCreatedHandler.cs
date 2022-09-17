using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Enums;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Events.External.Handlers;

public class SprintCreatedHandler : IEventHandler<SprintCreated>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;


    public SprintCreatedHandler(IProjectRepository projectRepository, ISprintRepository sprintRepository)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
    }

    public async Task HandleAsync(SprintCreated @event, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetAsync(@event.ProjectId);
        if (project is null) throw new ProjectNotFoundException(@event.ProjectId);

        var sprint = new Sprint(@event.Id, @event.ProjectId, SprintStatus.PLANNED);
        await _sprintRepository.AddAsync(sprint);

        project.SprintInsights.SprintAdded();
        await _projectRepository.UpdateAsync(project);
    }
}