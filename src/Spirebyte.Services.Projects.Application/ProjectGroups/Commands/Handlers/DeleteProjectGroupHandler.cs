using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.ProjectGroups.Events;
using Spirebyte.Services.Projects.Application.ProjectGroups.Exceptions;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands.Handlers;

internal sealed class DeleteProjectGroupHandler : ICommandHandler<DeleteProjectGroup>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionService _permissionService;
    private readonly IProjectGroupRepository _projectGroupRepository;

    public DeleteProjectGroupHandler(IProjectGroupRepository projectGroupRepository, IMessageBroker messageBroker,
        IPermissionService permissionService)
    {
        _projectGroupRepository = projectGroupRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
    }

    public async Task HandleAsync(DeleteProjectGroup command, CancellationToken cancellationToken = default)
    {
        if (!await _projectGroupRepository.ExistsAsync(command.Id))
            throw new ProjectGroupNotFoundException(command.Id);

        var projectGroup = await _projectGroupRepository.GetAsync(command.Id);
        if (!await _permissionService.HasPermission(projectGroup.ProjectId,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        await _projectGroupRepository.DeleteAsync(command.Id);
        await _messageBroker.SendAsync(new ProjectGroupDeleted(projectGroup), cancellationToken);
    }
}