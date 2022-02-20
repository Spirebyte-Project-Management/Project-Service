using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Projects.Application.Projects.Events;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Services.Interfaces;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Constants;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Objects;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;

namespace Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;

// Simple wrapper
internal sealed class CreateProjectHandler : ICommandHandler<CreateProject>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IAppContext _appContext;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public CreateProjectHandler(IUserRepository userRepository, IProjectRepository projectRepository,
        IMessageBroker messageBroker, IAppContext appContext)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _messageBroker = messageBroker;
        _appContext = appContext;
    }

    public async Task HandleAsync(CreateProject command, CancellationToken cancellationToken = default)
    {
        if (await _projectRepository.ExistsAsync(command.Id))
            throw new ProjectAlreadyExistsException(command.Id, _appContext.Identity.Id);

        if (!await _userRepository.ExistsAsync(_appContext.Identity.Id)) throw new UserNotFoundException(_appContext.Identity.Id);

        var project = new Project(command.Id, ProjectConstants.DefaultPermissionSchemeId, _appContext.Identity.Id,
            command.ProjectUserIds, command.InvitedUserIds, command.Pic, command.Title, command.Description,
            IssueInsights.Empty, SprintInsights.Empty, command.CreatedAt);
        await _projectRepository.AddAsync(project);
        await _messageBroker.PublishAsync(new ProjectCreated(project));
    }
}