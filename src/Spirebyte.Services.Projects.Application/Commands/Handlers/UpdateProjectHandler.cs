using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Partytitan.Convey.WindowsAzure.Blob.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Clients.Interfaces;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers
{
    internal sealed class UpdateProjectHandler : ICommandHandler<UpdateProject>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IIdentityApiHttpClient _identityApiHttpClient;
        private readonly ILogger<UpdateProjectHandler> _logger;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMessageBroker _messageBroker;

        public UpdateProjectHandler(IProjectRepository projectRepository, IIdentityApiHttpClient identityApiHttpClient, ILogger<UpdateProjectHandler> logger,
            IBlobStorageService blobStorageService, IMessageBroker messageBroker)
        {
            _projectRepository = projectRepository;
            _identityApiHttpClient = identityApiHttpClient;
            _logger = logger;
            _blobStorageService = blobStorageService;
            _messageBroker = messageBroker;
        }
        public async Task HandleAsync(UpdateProject command)
        {

            var project = await _projectRepository.GetAsync(command.Id);
            if (project is null)
            {
                throw new ProjectNotFoundException(command.Id);
            }

            var newInvitations = command.InvitedUserIds.Except(project.InvitedUserIds);
            foreach (var newInvitation in newInvitations)
            {
                var user = await _identityApiHttpClient.GetUserAsync(newInvitation);
                await _messageBroker.PublishAsync(new UserInvitedToProject(project.Id, user.Id, project.Title, user.Fullname, user.Email));
            }

            var picUrl = project.Pic;

            if (!string.IsNullOrWhiteSpace(command.File))
            {
                var mimeType = Extensions.GetMimeTypeFromBase64(command.File);
                var data = Extensions.GetDataFromBase64(command.File);
                var fileName = project.Id + "_" + DateTime.Now.ConvertToUnixTimestamp();

                var bytes = Convert.FromBase64String(data);
                Stream contents = new MemoryStream(bytes);
                var uri = await _blobStorageService.UploadFileBlobAsync(contents, mimeType, fileName);
                picUrl = uri.OriginalString;
            }

            project = new Project(project.Id, project.PermissionSchemeId, project.OwnerUserId, command.ProjectUserIds, command.InvitedUserIds, picUrl, command.Title, command.Description, project.IssueCount, project.CreatedAt);
            await _projectRepository.UpdateAsync(project);

            _logger.LogInformation($"Updated project with id: {project.Id}.");

            await _messageBroker.PublishAsync(new ProjectUpdated(project.Id));
        }
    }
}
