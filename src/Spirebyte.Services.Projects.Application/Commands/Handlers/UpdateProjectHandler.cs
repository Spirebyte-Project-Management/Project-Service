using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Partytitan.Convey.Minio.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Clients.Interfaces;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers;

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

    public async Task HandleAsync(UpdateProject command)
    {
        var project = await _projectRepository.GetAsync(command.Id);
        if (project is null) throw new ProjectNotFoundException(command.Id);

        // Check permissions
        if (!await _permissionService.HasPermission(command.Id, _appContext.Identity.Id,
                ProjectPermissionKeys.AdministerProject)) throw new ActionNotAllowedException();

        var newInvitations = command.InvitedUserIds.Except(project.InvitedUserIds);
        foreach (var newInvitation in newInvitations)
        {
            var user = await _identityApiHttpClient.GetUserAsync(newInvitation);
            await _messageBroker.PublishAsync(new UserInvitedToProject(project.Id, user.Id, project.Title,
                user.Fullname, user.Email));
        }

        var picUrl = project.Pic;

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

        project = new Project(project.Id, project.PermissionSchemeId, project.OwnerUserId, command.ProjectUserIds,
            command.InvitedUserIds, picUrl, command.Title, command.Description, project.IssueInsights,
            project.SprintInsights, project.CreatedAt);
        await _projectRepository.UpdateAsync(project);

        _logger.LogInformation($"Updated project with id: {project.Id}.");

        await _messageBroker.PublishAsync(new ProjectUpdated(project.Id));
    }
}