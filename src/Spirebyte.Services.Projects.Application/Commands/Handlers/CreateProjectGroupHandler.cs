using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers
{
    internal sealed class CreateProjectGroupHandler : ICommandHandler<CreateProjectGroup>
    {
        private readonly IProjectGroupRepository _projectGroupRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IPermissionService _permissionService;
        private readonly IAppContext _appContext;

        public CreateProjectGroupHandler(IProjectGroupRepository projectGroupRepository, IProjectRepository projectRepository,
            IMessageBroker messageBroker, IPermissionService permissionService, IAppContext appContext)
        {
            _projectGroupRepository = projectGroupRepository;
            _projectRepository = projectRepository;
            _messageBroker = messageBroker;
            _permissionService = permissionService;
            _appContext = appContext;
        }

        public async Task HandleAsync(CreateProjectGroup command)
        {
            if (!await _projectRepository.ExistsAsync(command.ProjectId))
            {
                throw new ProjectNotFoundException(command.ProjectId);
            }

            if (await _projectGroupRepository.ExistsWithNameAsync(command.Name))
            {
                throw new ProjectGroupAlreadyExistsException(command.Name);
            }

            if (!await _permissionService.HasPermission(command.ProjectId, _appContext.Identity.Id, ProjectPermissionKeys.AdministerProject))
            {
                throw new ActionNotAllowedException();
            }

            var projectGroup = new ProjectGroup(command.ProjectGroupId, command.ProjectId, command.Name, command.UserIds);
            await _projectGroupRepository.AddAsync(projectGroup);
            await _messageBroker.PublishAsync(new ProjectGroupCreated(projectGroup.Id));
        }
    }
}
