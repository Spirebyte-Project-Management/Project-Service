using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Projects.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;

// Simple wrapper
internal sealed class LeaveProjectHandler : ICommandHandler<LeaveProject>
{
    private readonly IAppContext _appContext;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public LeaveProjectHandler(IUserRepository userRepository, IProjectRepository projectRepository,
        IMessageBroker messageBroker, IAppContext appContext)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _appContext = appContext;
    }

    public async Task HandleAsync(LeaveProject command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _userRepository.ExistsAsync(_appContext.Identity.Id))
            throw new UserNotFoundException(_appContext.Identity.Id);

        var project = await _projectRepository.GetAsync(command.ProjectId);
        if (!project.InvitedUserIds.Contains(_appContext.Identity.Id))
            throw new UserNotInvitedException(_appContext.Identity.Id, command.ProjectId);

        project.LeaveProject(_appContext.Identity.Id);
        await _projectRepository.UpdateAsync(project);
        await _messageBroker.PublishAsync(new ProjectJoined(project.Id, _appContext.Identity.Id));
    }
}