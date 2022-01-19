using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.Commands;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Objects;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class JoinProjectTests : IDisposable
{
    private const string Exchange = "projects";
    private readonly ICommandHandler<JoinProject> _commandHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public JoinProjectTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<JoinProject>>();
    }

    public void Dispose()
    {
        _projectsMongoDbFixture.Dispose();
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task join_project_command_should_add_invited_user_to_project()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var invitedUserId = Guid.NewGuid();
        var title = "Title";
        var description = "description";

        var user = new User(ownerId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var invitedUser = new User(invitedUserId);
        await _usersMongoDbFixture.InsertAsync(invitedUser.AsDocument());

        var project = new Project(projectId, permissionSchemeId, ownerId, null, new[] { invitedUserId },
            "test.nl/image", title, description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new JoinProject(projectId, invitedUserId);

        // Check if exception is thrown

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrow();


        var updatedProject = await _projectsMongoDbFixture.GetAsync(projectId);

        updatedProject.Should().NotBeNull();
        updatedProject.ProjectUserIds.Should().Contain(i => i == invitedUserId);
    }

    [Fact]
    public async Task join_project_command_fails_when_no_project_with_id_exists()
    {
        var projectId = "key";
        var ownerId = Guid.NewGuid();
        var invitedUserId = Guid.NewGuid();
        var title = "Title";
        var description = "description";

        var user = new User(ownerId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var invitedUser = new User(invitedUserId);
        await _usersMongoDbFixture.InsertAsync(invitedUser.AsDocument());


        var command = new JoinProject(projectId, invitedUserId);

        // Check if exception is thrown

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().Throw<ProjectNotFoundException>();
    }

    [Fact]
    public async Task join_project_command_fails_when_invited_user_does_not_exist()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var invitedUserId = Guid.NewGuid();
        var title = "Title";
        var description = "description";

        var user = new User(ownerId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var project = new Project(projectId, permissionSchemeId, ownerId, null, new[] { invitedUserId },
            "test.nl/image", title, description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new JoinProject(projectId, invitedUserId);

        // Check if exception is thrown

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().Throw<UserNotFoundException>();
    }

    [Fact]
    public async Task join_project_command_fails_when_user_is_not_invited()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var invitedUserId = Guid.NewGuid();
        var title = "Title";
        var description = "description";

        var user = new User(ownerId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var invitedUser = new User(invitedUserId);
        await _usersMongoDbFixture.InsertAsync(invitedUser.AsDocument());

        var project = new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title,
            description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new JoinProject(projectId, invitedUserId);

        // Check if exception is thrown

        _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().Throw<UserNotInvitedException>();
    }
}