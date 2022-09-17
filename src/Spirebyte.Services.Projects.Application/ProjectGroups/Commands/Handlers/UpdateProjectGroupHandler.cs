using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.ProjectGroups.Events;
using Spirebyte.Services.Projects.Application.ProjectGroups.Exceptions;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Changes;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands.Handlers;

internal sealed class UpdateProjectGroupHandler : ICommandHandler<UpdateProjectGroup>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionService _permissionService;
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectGroupHandler(IProjectGroupRepository projectGroupRepository,
        IProjectRepository projectRepository,
        IMessageBroker messageBroker, IPermissionService permissionService)
    {
        _projectGroupRepository = projectGroupRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
    }

    public async Task HandleAsync(UpdateProjectGroup command, CancellationToken cancellationToken = default)
    {
        if (!await _projectGroupRepository.ExistsAsync(command.Id))
            throw new ProjectGroupNotFoundException(command.Id);

        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _permissionService.HasPermission(command.ProjectId,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var projectGroup = await _projectGroupRepository.GetAsync(command.Id);
        var updatedProjectGroup = new ProjectGroup(projectGroup.Id, projectGroup.ProjectId, projectGroup.Name,
            command.UserIds);

        await _projectGroupRepository.UpdateAsync(updatedProjectGroup);

        if (ChangedFieldsHelper.HasChanges(projectGroup, updatedProjectGroup))
        {
            await _messageBroker.SendAsync(new ProjectGroupUpdated(updatedProjectGroup, projectGroup), cancellationToken);
        }
    }
}