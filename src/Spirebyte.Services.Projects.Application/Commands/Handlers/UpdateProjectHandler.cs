using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Partytitan.Convey.WindowsAzure.Blob.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers
{
    internal sealed class UpdateProjectHandler : ICommandHandler<UpdateProject>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<UpdateProjectHandler> _logger;
        private readonly IBlobStorageService _blobStorageService;

        public UpdateProjectHandler(IProjectRepository projectRepository, ILogger<UpdateProjectHandler> logger,
            IBlobStorageService blobStorageService)
        {
            _projectRepository = projectRepository;
            _logger = logger;
            _blobStorageService = blobStorageService;
        }
        public async Task HandleAsync(UpdateProject command)
        {

            var project = await _projectRepository.GetAsync(command.Key);
            if (project is null)
            {
                throw new ProjectNotFoundException(command.ProjectId);
            }

            string picUrl = command.Pic;

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

            project = new Project(project.Id, project.OwnerUserId, command.ProjectUserIds, project.Key, picUrl, command.Title, command.Description, project.CreatedAt);
            await _projectRepository.UpdateAsync(project);

            _logger.LogInformation($"Updated project with id: {project.Id}.");
        }
    }
}
