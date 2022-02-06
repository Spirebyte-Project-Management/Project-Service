using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.Projects.Commands;
using Spirebyte.Services.Projects.Application.Projects.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Objects;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class UpdateProjectTests : IDisposable
{
    private const string Exchange = "projects";
    private readonly ICommandHandler<UpdateProject> _commandHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;

    public UpdateProjectTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<UpdateProject>>();
    }

    public void Dispose()
    {
        _projectsMongoDbFixture.Dispose();
        _usersMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task update_project_command_should_modify_project_with_given_data()
    {
        var projectId = "key";
        var permissionSchemeId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var title = "Title";
        var updatedTitle = "UpdatedTitle";
        var description = "description";
        var updatedDescription = "UpdatedDescription";

        var user = new User(ownerId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var project = new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title,
            description, IssueInsights.Empty, SprintInsights.Empty, DateTime.UtcNow);
        await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new UpdateProject(projectId, null, null, "test.nl/image", null, updatedTitle,
            updatedDescription);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var updatedProject = await _projectsMongoDbFixture.GetAsync(command.Id);

        updatedProject.Should().NotBeNull();
        updatedProject.Id.Should().Be(projectId);
        updatedProject.OwnerUserId.Should().Be(ownerId);
        updatedProject.Title.Should().Be(updatedTitle);
        updatedProject.Description.Should().Be(updatedDescription);
    }

    [Fact]
    public async Task update_project_command_fails_when_project_does_not_exists_in_database()
    {
        var projectId = "key";
        var ownerId = Guid.NewGuid();
        var title = "Title";
        var updatedTitle = "UpdatedTitle";
        var description = "description";
        var updatedDescription = "UpdatedDescription";

        // Add owner
        var user = new User(ownerId);
        await _usersMongoDbFixture.InsertAsync(user.AsDocument());

        var command = new UpdateProject(projectId, null, null, "test.nl/image", null, updatedTitle,
            updatedDescription);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}