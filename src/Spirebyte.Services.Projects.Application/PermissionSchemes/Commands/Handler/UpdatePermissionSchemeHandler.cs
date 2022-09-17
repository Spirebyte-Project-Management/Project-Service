using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Events;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands.Handler;

internal sealed class UpdatePermissionSchemeHandler : ICommandHandler<UpdatePermissionScheme>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;

    public UpdatePermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository,
        IProjectRepository projectRepository,
        IMessageBroker messageBroker, IPermissionService permissionService)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
    }

    public async Task HandleAsync(UpdatePermissionScheme command, CancellationToken cancellationToken = default)
    {
        if (!await _permissionSchemeRepository.ExistsAsync(command.Id))
            throw new ProjectPermissionSchemeNotFoundException(command.Id);

        var projectPermissionScheme = await _permissionSchemeRepository.GetAsync(command.Id);

        if (!await _projectRepository.ExistsAsync(projectPermissionScheme.ProjectId))
            throw new ProjectNotFoundException(projectPermissionScheme.ProjectId);

        if (!await _permissionService.HasPermission(projectPermissionScheme.ProjectId, ProjectPermissionKeys.AdministerProject)) 
            throw new ActionNotAllowedException();

        var updatedProjectPermissionScheme = new PermissionScheme(projectPermissionScheme.Id,
            projectPermissionScheme.ProjectId, command.Name, command.Description, command.Permissions);

        await _permissionSchemeRepository.UpdateAsync(updatedProjectPermissionScheme);
        await _messageBroker.SendAsync(
            new ProjectPermissionSchemeUpdated(updatedProjectPermissionScheme, projectPermissionScheme), cancellationToken);
    }
}