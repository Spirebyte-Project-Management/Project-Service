﻿using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Events;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.PermissionSchemes.Commands.Handler;

internal sealed class CreateCustomPermissionSchemeHandler : ICommandHandler<CreateCustomPermissionScheme>
{
    private readonly IAppContext _appContext;
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;


    public CreateCustomPermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository,
        IProjectRepository projectRepository, IMessageBroker messageBroker, IPermissionService permissionService,
        IAppContext appContext)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
        _appContext = appContext;
    }

    public async Task HandleAsync(CreateCustomPermissionScheme command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _permissionService.HasPermission(command.ProjectId, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var project = await _projectRepository.GetAsync(command.ProjectId);

        var defaultPermissionSchemeCopy =
            await _permissionSchemeRepository.GetAsync(ProjectConstants.DefaultPermissionSchemeId);
        defaultPermissionSchemeCopy.Id = command.Id;
        defaultPermissionSchemeCopy.ProjectId = command.ProjectId;
        defaultPermissionSchemeCopy.Description = $"Copy of {defaultPermissionSchemeCopy.Name}";
        defaultPermissionSchemeCopy.Name = $"{project.Title} Permission scheme";

        await _permissionSchemeRepository.AddAsync(defaultPermissionSchemeCopy);

        project.SetPermissionSchemeId(command.Id);
        await _projectRepository.UpdateAsync(project);

        await _messageBroker.PublishAsync(new CustomPermissionSchemeCreated(defaultPermissionSchemeCopy));
    }
}