using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers
{
    // Simple wrapper
    internal sealed class JoinProjectHandler : ICommandHandler<JoinProject>
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageBroker _messageBroker;

        public JoinProjectHandler(IUserRepository userRepository, IProjectRepository projectRepository,
            IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(JoinProject command)
        {
            if (!await _projectRepository.ExistsWithKeyAsync(command.Key))
            {
                throw new ProjectNotFoundException(command.Key);
            }

            if (!await _userRepository.ExistsAsync(command.UserId))
            {
                throw new UserNotFoundException(command.UserId);
            }

            var project = await _projectRepository.GetAsync(command.Key);
            if (!project.InvitedUserIds.Contains(command.UserId))
            {
                throw new UserNotInvitedException(command.UserId, command.Key);
            }

            project.JoinProject(command.UserId);

            await _projectRepository.UpdateAsync(project);
            await _messageBroker.PublishAsync(new ProjectJoined(project.Id, project.Key, command.UserId));
        }
    }
}
