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

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands.Handlers;

internal sealed class CreateProjectGroupHandler : ICommandHandler<CreateProjectGroup>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionService _permissionService;
    private readonly IProjectGroupRepository _projectGroupRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateProjectGroupHandler(IProjectGroupRepository projectGroupRepository,
        IProjectRepository projectRepository,
        IMessageBroker messageBroker, IPermissionService permissionService)
    {
        _projectGroupRepository = projectGroupRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
    }

    public async Task HandleAsync(CreateProjectGroup command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (await _projectGroupRepository.ExistsWithNameAsync(command.Name))
            throw new ProjectGroupAlreadyExistsException(command.Name);

        if (!await _permissionService.HasPermission(command.ProjectId,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var projectGroup =
            new ProjectGroup(command.ProjectGroupId, command.ProjectId, command.Name, command.UserIds);
        await _projectGroupRepository.AddAsync(projectGroup);
        await _messageBroker.SendAsync(new ProjectGroupCreated(projectGroup.Id, projectGroup.ProjectId,
            projectGroup.Name, projectGroup.UserIds), cancellationToken);
    }
}