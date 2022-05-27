using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Partytitan.Convey.Minio.Services.Interfaces;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;

internal sealed class UpdateProjectHandler : ICommandHandler<UpdateProject>
{
    private readonly IAppContext _appContext;
    private readonly IIdentityApiHttpClient _identityApiHttpClient;
    private readonly ILogger<UpdateProjectHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IMinioService _minioService;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectHandler(IProjectRepository projectRepository, IIdentityApiHttpClient identityApiHttpClient,
        ILogger<UpdateProjectHandler> logger,
        IMinioService minioService, IPermissionService permissionService, IMessageBroker messageBroker,
        IAppContext appContext)
    {
        _projectRepository = projectRepository;
        _identityApiHttpClient = identityApiHttpClient;
        _logger = logger;
        _minioService = minioService;
        _permissionService = permissionService;
        _messageBroker = messageBroker;
        _appContext = appContext;
    }

    public async Task HandleAsync(UpdateProject command, CancellationToken cancellationToken = default)
    {
        var currentProject = await _projectRepository.GetAsync(command.Id);
        if (currentProject is null) throw new ProjectNotFoundException(command.Id);

        // Check permissions
        if (!await _permissionService.HasPermission(command.Id, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var newInvitations = command.InvitedUserIds.Except(currentProject.InvitedUserIds);
        foreach (var newInvitation in newInvitations)
        {
            var user = await _identityApiHttpClient.GetUserAsync(newInvitation);
            await _messageBroker.PublishAsync(new UserInvitedToProject(currentProject.Id, Guid.Parse(user.Id), currentProject.Title,
                user.PreferredUsername, user.Email));
        }

        var picUrl = currentProject.Pic;

        if (!string.IsNullOrWhiteSpace(command.File))
        {
            if (command.File == "delete")
            {
                picUrl = string.Empty;
            }
            else
            {
                var mimeType = Extensions.GetMimeTypeFromBase64(command.File);
                var data = Extensions.GetDataFromBase64(command.File);
                var fileName = _appContext.Identity.Id + "_" + DateTime.Now.ConvertToUnixTimestamp();

                var bytes = Convert.FromBase64String(data);
                Stream contents = new MemoryStream(bytes);
                picUrl = await _minioService.UploadFileAsync(contents, mimeType, fileName);
            }
        }

        var newProject = new Project(currentProject.Id, currentProject.PermissionSchemeId, currentProject.OwnerUserId,
            command.ProjectUserIds,
            command.InvitedUserIds, picUrl, command.Title, command.Description, currentProject.IssueInsights,
            currentProject.SprintInsights, currentProject.CreatedAt);
        await _projectRepository.UpdateAsync(newProject);

        _logger.LogInformation($"Updated project with id: {newProject.Id}.");

        await _messageBroker.PublishAsync(new ProjectUpdated(newProject, currentProject));
    }
}