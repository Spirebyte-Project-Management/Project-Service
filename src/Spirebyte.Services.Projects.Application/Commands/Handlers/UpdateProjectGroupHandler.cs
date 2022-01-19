using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers;

internal sealed class UpdateProjectGroupHandler : ICommandHandler<UpdateProjectGroup>
{
    private const int DefaultPermissionSchemeId = 1;
    private readonly IAppContext _appContext;
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionService _permissionService;
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectGroupHandler(IProjectGroupRepository projectGroupRepository,
        IProjectRepository projectRepository,
        IMessageBroker messageBroker, IPermissionService permissionService, IAppContext appContext)
    {
        _projectGroupRepository = projectGroupRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
        _appContext = appContext;
    }

    public async Task HandleAsync(UpdateProjectGroup command)
    {
        if (!await _projectGroupRepository.ExistsAsync(command.Id))
            throw new ProjectGroupNotFoundException(command.Id);

        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _permissionService.HasPermission(command.ProjectId, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var projectGroup = await _projectGroupRepository.GetAsync(command.Id);
        var updatedProjectGroup = new ProjectGroup(projectGroup.Id, projectGroup.ProjectId, projectGroup.Name,
            command.UserIds);

        await _projectGroupRepository.UpdateAsync(updatedProjectGroup);
        await _messageBroker.PublishAsync(new ProjectGroupUpdated(projectGroup.Id));
    }
}