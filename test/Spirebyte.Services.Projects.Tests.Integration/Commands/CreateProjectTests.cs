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

public class CreateProjectTests : TestBase
{
    private readonly TestMessageBroker _messageBroker;

    private readonly ICommandHandler<CreateProject> _commandHandler;
    
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IContextAccessor _contextAccessor;

    
    public CreateProjectTests(
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

        _commandHandler = new CreateProjectHandler(_userRepository, _projectRepository, _messageBroker, _contextAccessor);
    }


    [Fact]
    public async Task create_project_command_should_add_project_with_given_data_to_database()
    {
        var fakedProject = ProjectFaker.Instance.Generate();

        await _userRepository.AddAsync(new User(fakedProject.OwnerUserId));
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", fakedProject.OwnerUserId.ToString()));

        var command = new CreateProject(fakedProject.Id, fakedProject.ProjectUserIds, fakedProject.InvitedUserIds,
            "test.nl/image", fakedProject.Title, fakedProject.Description);

        // Check if exception is thrown
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var project = await ProjectsMongoDbFixture.GetAsync(command.Id);

        project.Should().NotBeNull();
        project.Id.Should().Be(fakedProject.Id);
        project.Title.Should().Be(fakedProject.Title);
        project.Description.Should().Be(fakedProject.Description  );
    }

    [Fact]
    public async Task create_project_command_fails_when_project_with_key_already_exists_in_database()
    {
        var fakedProject = ProjectFaker.Instance.Generate();
        await UsersMongoDbFixture.AddAsync(new User(fakedProject.OwnerUserId).AsDocument());
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", fakedProject.OwnerUserId.ToString()));

        // Add project
        await ProjectsMongoDbFixture.AddAsync(fakedProject.AsDocument());

        var command = new CreateProject(fakedProject.Id, fakedProject.ProjectUserIds, fakedProject.InvitedUserIds,
            "test.nl/image", fakedProject.Title, fakedProject.Description);
        
        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectAlreadyExistsException>();
    }

    [Fact]
    public async Task create_project_command_fails_when_owner_does_not_exist()
    {
        var fakedProject = ProjectFaker.Instance.Generate();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", fakedProject.OwnerUserId.ToString()));

        var command = new CreateProject(fakedProject.Id, fakedProject.ProjectUserIds, fakedProject.InvitedUserIds,
            "test.nl/image", fakedProject.Title, fakedProject.Description);

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<UserNotFoundException>();
    }
}