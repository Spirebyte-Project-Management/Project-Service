using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.ProjectGroups.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands.Handler;

internal sealed class DeletePermissionSchemeHandler : ICommandHandler<DeletePermissionScheme>
{
    private const int DefaultPermissionSchemeId = 1;
    private readonly IAppContext _appContext;
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;

    public DeletePermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository,
        IProjectRepository projectRepository, IMessageBroker messageBroker, IPermissionService permissionService,
        IAppContext appContext)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
        _appContext = appContext;
    }

    public async Task HandleAsync(DeletePermissionScheme command, CancellationToken cancellationToken = default)
    {
        if (!await _permissionSchemeRepository.ExistsAsync(command.Id))
            throw new ProjectPermissionSchemeNotFoundException(command.Id);

        var permissionScheme = await _permissionSchemeRepository.GetAsync(command.Id);
        if (!await _projectRepository.ExistsAsync(permissionScheme.ProjectId))
            throw new ProjectNotFoundException(permissionScheme.ProjectId);

        if (!await _permissionService.HasPermission(permissionScheme.ProjectId, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var project = await _projectRepository.GetAsync(permissionScheme.ProjectId);
        project.SetPermissionSchemeId(ProjectConstants.DefaultPermissionSchemeId);
        await _projectRepository.UpdateAsync(project);

        await _permissionSchemeRepository.DeleteAsync(command.Id);
        await _messageBroker.PublishAsync(new ProjectGroupDeleted(command.Id));
    }
}