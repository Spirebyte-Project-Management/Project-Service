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
    // Simple wrapper
    internal sealed class CreateProjectHandler : ICommandHandler<CreateProject>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageBroker _messageBroker;

        public CreateProjectHandler(IUserRepository userRepository, IProjectRepository projectRepository,
            IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateProject command)
        {
            if (await _projectRepository.ExistsAsync(command.Id))
            {
                throw new ProjectAlreadyExistsException(command.Id, command.OwnerId);
            }

            if (!(await _userRepository.ExistsAsync(command.OwnerId)))
            {
                throw new UserNotFoundException(command.OwnerId);
            }

            var project = new Project(command.Id, ProjectConstants.DefaultPermissionSchemeId, command.OwnerId, command.ProjectUserIds, command.InvitedUserIds, command.Pic, command.Title, command.Description, 0, command.CreatedAt);
            await _projectRepository.AddAsync(project);
            await _messageBroker.PublishAsync(new ProjectCreated(project.Id));
        }
    }
}
