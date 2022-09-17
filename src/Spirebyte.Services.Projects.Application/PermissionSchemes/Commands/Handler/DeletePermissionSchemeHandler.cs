using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Events;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands.Handler;

internal sealed class DeletePermissionSchemeHandler : ICommandHandler<DeletePermissionScheme>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;

    public DeletePermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository,
        IProjectRepository projectRepository, IMessageBroker messageBroker, IPermissionService permissionService)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
    }

    public async Task HandleAsync(DeletePermissionScheme command, CancellationToken cancellationToken = default)
    {
        if (!await _permissionSchemeRepository.ExistsAsync(command.Id))
            throw new ProjectPermissionSchemeNotFoundException(command.Id);

        var permissionScheme = await _permissionSchemeRepository.GetAsync(command.Id);
        if (!await _projectRepository.ExistsAsync(permissionScheme.ProjectId))
            throw new ProjectNotFoundException(permissionScheme.ProjectId);

        if (!await _permissionService.HasPermission(permissionScheme.ProjectId, ProjectPermissionKeys.AdministerProject)) 
            throw new ActionNotAllowedException();

        var project = await _projectRepository.GetAsync(permissionScheme.ProjectId);
        project.SetPermissionSchemeId(ProjectConstants.DefaultPermissionSchemeId);
        await _projectRepository.UpdateAsync(project);

        await _permissionSchemeRepository.DeleteAsync(command.Id);
        await _messageBroker.SendAsync(new ProjectPermissionSchemeDeleted(permissionScheme), cancellationToken);
    }
}