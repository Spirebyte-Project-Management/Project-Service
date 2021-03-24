using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Events;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Commands.Handlers
{
    internal sealed class CreateProjectGroupHandler : ICommandHandler<CreateProjectGroup>
    {
        private readonly IProjectGroupRepository _projectGroupRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageBroker _messageBroker;

        private const int DefaultPermissionSchemeId = 1;

        public CreateProjectGroupHandler(IProjectGroupRepository projectGroupRepository, IProjectRepository projectRepository,
            IMessageBroker messageBroker)
        {
            _projectGroupRepository = projectGroupRepository;
            _projectRepository = projectRepository;
            _messageBroker = messageBroker;
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

            var projectGroup = new ProjectGroup(command.ProjectGroupId, command.ProjectId, command.Name, command.UserIds);
            await _projectGroupRepository.AddAsync(projectGroup);
            await _messageBroker.PublishAsync(new ProjectGroupCreated(projectGroup.Id));
        }
    }
}
