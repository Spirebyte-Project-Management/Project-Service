using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.Commands.Handlers;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Application.Users.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Repositories;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Projects.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Commands;

public class LeaveProjectTests : TestBase
{
    private readonly TestMessageBroker _messageBroker;

    private readonly ICommandHandler<LeaveProject> _commandHandler;
    
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IContextAccessor _contextAccessor;

    
    public LeaveProjectTests(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(issuesMongoDbFixture, permissionSchemesMongoDbFixture, projectGroupsMongoDbFixture, projectsMongoDbFixture, sprintsMongoDbFixture, usersMongoDbFixture)
    {
        _messageBroker = new TestMessageBroker();

        _userRepository = new UserRepository(UsersMongoDbFixture);
        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);

        _contextAccessor = Substitute.For<IContextAccessor>();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", Guid.NewGuid().ToString()));

        _commandHandler = new LeaveProjectHandler(_userRepository, _projectRepository, _messageBroker, _contextAccessor);
    }
    
    [Fact]
    public async Task leave_project_command_should_remove_invited_user_from_project()
    {
        var fakedProject = ProjectFaker.Instance.Generate();

        await UsersMongoDbFixture.AddAsync(new User(fakedProject.OwnerUserId).AsDocument());

        var invitedUser = new User(Guid.NewGuid());
        await UsersMongoDbFixture.AddAsync(invitedUser.AsDocument());
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id",
            "some-message-id", "some-causation-id", invitedUser.Id.ToString()));

        fakedProject.InvitedUserIds.Add(invitedUser.Id);
        await ProjectsMongoDbFixture.AddAsync(fakedProject.AsDocument());
        
        var command = new LeaveProject(fakedProject.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();

        var updatedProject = await ProjectsMongoDbFixture.GetAsync(fakedProject.Id);

        updatedProject.Should().NotBeNull();
        updatedProject.InvitedUserIds.Should().NotContain(i => i == invitedUser.Id);
        updatedProject.ProjectUserIds.Should().NotContain(i => i == invitedUser.Id);
    }

    [Fact]
    public async Task leave_project_command_fails_when_no_project_with_id_exists()
    {
        var fakedProject = ProjectFaker.Instance.Generate();

        await UsersMongoDbFixture.AddAsync(new User(fakedProject.OwnerUserId).AsDocument());
        
        var invitedUser = new User(Guid.NewGuid());
        await UsersMongoDbFixture.AddAsync(invitedUser.AsDocument());
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", invitedUser.Id.ToString()));

        var command = new LeaveProject(fakedProject.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Fact]
    public async Task leave_project_command_fails_when_invited_user_does_not_exist()
    {
        var fakedProject = ProjectFaker.Instance.Generate();

        await UsersMongoDbFixture.AddAsync(new User(fakedProject.OwnerUserId).AsDocument());
        
        var invitedUser = new User(Guid.NewGuid());
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", invitedUser.Id.ToString()));
        
        fakedProject.InvitedUserIds.Add(invitedUser.Id);
        await ProjectsMongoDbFixture.AddAsync(fakedProject.AsDocument());
        
        var command = new LeaveProject(fakedProject.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task leave_project_command_fails_when_user_is_not_invited()
    {
        var fakedProject = ProjectFaker.Instance.Generate();

        await UsersMongoDbFixture.AddAsync(new User(fakedProject.OwnerUserId).AsDocument());

        var invitedUser = new User(Guid.NewGuid());
        await UsersMongoDbFixture.AddAsync(invitedUser.AsDocument());
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id",
            "some-message-id", "some-causation-id", invitedUser.Id.ToString()));

        await ProjectsMongoDbFixture.AddAsync(fakedProject.AsDocument());
        
        var command = new LeaveProject(fakedProject.Id);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<UserNotInvitedException>();
    }
}