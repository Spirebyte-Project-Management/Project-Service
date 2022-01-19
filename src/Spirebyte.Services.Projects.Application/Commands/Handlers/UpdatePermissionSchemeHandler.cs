using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers;

internal sealed class UpdatePermissionSchemeHandler : ICommandHandler<UpdatePermissionScheme>
{
    private const int DefaultPermissionSchemeId = 1;
    private readonly IAppContext _appContext;
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;

    public UpdatePermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository,
        IProjectRepository projectRepository,
        IMessageBroker messageBroker, IPermissionService permissionService, IAppContext appContext)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
        _appContext = appContext;
    }

    public async Task HandleAsync(UpdatePermissionScheme command)
    {
        if (!await _permissionSchemeRepository.ExistsAsync(command.Id))
            throw new ProjectPermissionSchemeNotFoundException(command.Id);

        var projectPermissionScheme = await _permissionSchemeRepository.GetAsync(command.Id);

        if (!await _projectRepository.ExistsAsync(projectPermissionScheme.ProjectId))
            throw new ProjectNotFoundException(projectPermissionScheme.ProjectId);

        if (!await _permissionService.HasPermission(projectPermissionScheme.ProjectId, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var updatedProjectPermissionScheme = new PermissionScheme(projectPermissionScheme.Id,
            projectPermissionScheme.ProjectId, command.Name, command.Description, command.Permissions);

        await _permissionSchemeRepository.UpdateAsync(updatedProjectPermissionScheme);
        await _messageBroker.PublishAsync(new ProjectPermissionSchemeUpdated(updatedProjectPermissionScheme.Id));
    }
}