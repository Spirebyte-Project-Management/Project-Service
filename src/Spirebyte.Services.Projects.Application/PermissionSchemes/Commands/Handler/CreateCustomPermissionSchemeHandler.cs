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

internal sealed class CreateCustomPermissionSchemeHandler : ICommandHandler<CreateCustomPermissionScheme>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IPermissionSchemeRepository _permissionSchemeRepository;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;


    public CreateCustomPermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository,
        IProjectRepository projectRepository, IMessageBroker messageBroker, IPermissionService permissionService)
    {
        _permissionSchemeRepository = permissionSchemeRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _permissionService = permissionService;
    }

    public async Task HandleAsync(CreateCustomPermissionScheme command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _permissionService.HasPermission(command.ProjectId, ProjectPermissionKeys.AdministerProject)) 
            throw new ActionNotAllowedException();

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

        await _messageBroker.SendAsync(new CustomPermissionSchemeCreated(defaultPermissionSchemeCopy), cancellationToken);
    }
}