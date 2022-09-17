using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Objects;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;

internal sealed class CreateProjectHandler : ICommandHandler<CreateProject>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public CreateProjectHandler(IUserRepository userRepository, IProjectRepository projectRepository,
        IMessageBroker messageBroker, IContextAccessor contextAccessor)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(CreateProject command, CancellationToken cancellationToken = default)
    {
        var ownerId = _contextAccessor.Context.GetUserId();
        
        if (await _projectRepository.ExistsAsync(command.Id))
            throw new ProjectAlreadyExistsException(command.Id, ownerId);

        if (!await _userRepository.ExistsAsync(ownerId))
            throw new UserNotFoundException(ownerId);

        var project = new Project(command.Id, ProjectConstants.DefaultPermissionSchemeId, ownerId,
            command.ProjectUserIds, command.InvitedUserIds, command.Pic, command.Title, command.Description,
            IssueInsights.Empty, SprintInsights.Empty, command.CreatedAt);
        await _projectRepository.AddAsync(project);
        await _messageBroker.SendAsync(new ProjectCreated(project), cancellationToken);
    }
}