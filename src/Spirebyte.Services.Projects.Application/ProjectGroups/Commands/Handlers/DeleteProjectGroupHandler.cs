﻿using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.ProjectGroups.Events;
using Spirebyte.Services.Projects.Application.ProjectGroups.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.ProjectGroups.Commands.Handlers;

internal sealed class DeleteProjectGroupHandler : ICommandHandler<DeleteProjectGroup>
{
    private const int DefaultPermissionSchemeId = 1;
    private readonly IAppContext _appContext;
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionService _permissionService;
    private readonly IProjectGroupRepository _projectGroupRepository;

    public DeleteProjectGroupHandler(IProjectGroupRepository projectGroupRepository, IMessageBroker messageBroker,
        IPermissionService permissionService, IAppContext appContext)
    {
        _projectGroupRepository = projectGroupRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
        _appContext = appContext;
    }

    public async Task HandleAsync(DeleteProjectGroup command, CancellationToken cancellationToken = default)
    {
        if (!await _projectGroupRepository.ExistsAsync(command.Id))
            throw new ProjectGroupNotFoundException(command.Id);

        var projectGroup = await _projectGroupRepository.GetAsync(command.Id);
        if (!await _permissionService.HasPermission(projectGroup.ProjectId, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        await _projectGroupRepository.DeleteAsync(command.Id);
        await _messageBroker.PublishAsync(new ProjectGroupDeleted(projectGroup));
    }
}