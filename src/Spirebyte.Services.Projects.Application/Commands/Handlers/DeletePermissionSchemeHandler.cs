using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers
{
    internal sealed class DeletePermissionSchemeHandler : ICommandHandler<DeletePermissionScheme>
    {
        private readonly IPermissionSchemeRepository _permissionSchemeRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IPermissionService _permissionService;
        private readonly IAppContext _appContext;

        private const int DefaultPermissionSchemeId = 1;

        public DeletePermissionSchemeHandler(IPermissionSchemeRepository permissionSchemeRepository, IProjectRepository projectRepository, IMessageBroker messageBroker, IPermissionService permissionService, IAppContext appContext)
        {
            _permissionSchemeRepository = permissionSchemeRepository;
            _projectRepository = projectRepository;
            _messageBroker = messageBroker;
            _permissionService = permissionService;
            _appContext = appContext;
        }

        public async Task HandleAsync(DeletePermissionScheme command)
        {
            if (!await _permissionSchemeRepository.ExistsAsync(command.Id))
            {
                throw new ProjectPermissionSchemeNotFoundException(command.Id);
            }

            var permissionScheme = await _permissionSchemeRepository.GetAsync(command.Id);
            if (!await _projectRepository.ExistsAsync(permissionScheme.ProjectId))
            {
                throw new ProjectNotFoundException(permissionScheme.ProjectId);
            }

            if (!await _permissionService.HasPermission(permissionScheme.ProjectId, _appContext.Identity.Id, ProjectPermissionKeys.AdministerProject))
            {
                throw new ActionNotAllowedException();
            }

            var project = await _projectRepository.GetAsync(permissionScheme.ProjectId);
            project.SetPermissionSchemeId(ProjectConstants.DefaultPermissionSchemeId);
            await _projectRepository.UpdateAsync(project);

            await _permissionSchemeRepository.DeleteAsync(command.Id);
            await _messageBroker.PublishAsync(new ProjectGroupDeleted(command.Id));
        }
    }
}
