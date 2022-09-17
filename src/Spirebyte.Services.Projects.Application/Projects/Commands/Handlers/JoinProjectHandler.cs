using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Repositories;

namespace Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;

internal sealed class JoinProjectHandler : ICommandHandler<JoinProject>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public JoinProjectHandler(IUserRepository userRepository, IProjectRepository projectRepository,
        IMessageBroker messageBroker, IContextAccessor contextAccessor)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(JoinProject command, CancellationToken cancellationToken = default)
    {
        var invitedUserId = _contextAccessor.Context.GetUserId();

        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _userRepository.ExistsAsync(invitedUserId))
            throw new UserNotFoundException(invitedUserId);

        var project = await _projectRepository.GetAsync(command.ProjectId);
        if (!project.InvitedUserIds.Contains(invitedUserId))
            throw new UserNotInvitedException(invitedUserId, command.ProjectId);

        project.JoinProject(invitedUserId);

        await _projectRepository.UpdateAsync(project);
        await _messageBroker.SendAsync(new ProjectJoined(project.Id, invitedUserId), cancellationToken);
    }
}