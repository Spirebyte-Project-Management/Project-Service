using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spirebyte.Framework.FileStorage.S3;
using Spirebyte.Framework.FileStorage.S3.Services;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Exceptions;
using Spirebyte.Services.Projects.Application.PermissionSchemes.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Projects.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Clients.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Changes;

namespace Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;

internal sealed class UpdateProjectHandler : ICommandHandler<UpdateProject>
{
    private readonly IIdentityApiHttpClient _identityApiHttpClient;
    private readonly ILogger<UpdateProjectHandler> _logger;
    private readonly IMessageBroker _messageBroker;
    private readonly IS3Service _s3Service;
    private readonly IPermissionService _permissionService;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectHandler(IProjectRepository projectRepository, IIdentityApiHttpClient identityApiHttpClient,
        ILogger<UpdateProjectHandler> logger, IS3Service s3Service, IPermissionService permissionService,
        IMessageBroker messageBroker)
    {
        _projectRepository = projectRepository;
        _identityApiHttpClient = identityApiHttpClient;
        _logger = logger;
        _s3Service = s3Service;
        _permissionService = permissionService;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(UpdateProject command, CancellationToken cancellationToken = default)
    {
        var currentProject = await _projectRepository.GetAsync(command.Id);
        if (currentProject is null) throw new ProjectNotFoundException(command.Id);

        // Check permissions
        if (!await _permissionService.HasPermission(command.Id, ProjectPermissionKeys.AdministerProject)) 
            throw new ActionNotAllowedException();

        var newInvitations = command.InvitedUserIds.Except(currentProject.InvitedUserIds);
        foreach (var newInvitation in newInvitations)
        {
            var user = await _identityApiHttpClient.GetUserAsync(newInvitation);
            await _messageBroker.SendAsync(new UserInvitedToProject(currentProject.Id, Guid.Parse(user.Id), currentProject.Title,
                user.PreferredUsername, user.Email), cancellationToken);
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
                var fileName = currentProject.Id + "_" + DateTime.Now.ConvertToUnixTimestamp();
                picUrl = await _s3Service.UploadImageFromBase64Async(command.File, fileName);
            }
        }

        var newProject = new Project(currentProject.Id, currentProject.PermissionSchemeId, currentProject.OwnerUserId,
            command.ProjectUserIds,
            command.InvitedUserIds, picUrl, command.Title, command.Description, currentProject.IssueInsights,
            currentProject.SprintInsights, currentProject.CreatedAt);
        await _projectRepository.UpdateAsync(newProject);

        _logger.LogInformation($"Updated project with id: {newProject.Id}.");

        if (ChangedFieldsHelper.HasChanges(newProject, currentProject))
        {
            await _messageBroker.SendAsync(new ProjectUpdated(newProject, currentProject), cancellationToken);
        }
    }
}