using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.Commands;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class CreateProjectTests : IDisposable
    {
        public CreateProjectTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateProject>>();
        }

        public void Dispose()
        {
            _projectsMongoDbFixture.Dispose();
            _usersMongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, string> _projectsMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<CreateProject> _commandHandler;


        [Fact]
        public async Task create_project_command_should_add_project_with_given_data_to_database()
        {
            var projectId = "key";
            var ownerId = Guid.NewGuid();
            var title = "Title";
            var description = "description";

            var user = new User(ownerId);
            await _usersMongoDbFixture.InsertAsync(user.AsDocument());


            var command = new CreateProject(projectId, ownerId, null, null, "test.nl/image", title, description);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var project = await _projectsMongoDbFixture.GetAsync(command.Id);

            project.Should().NotBeNull();
            project.Id.Should().Be(projectId);
            project.OwnerUserId.Should().Be(ownerId);
            project.Title.Should().Be(title);
            project.Description.Should().Be(description);
        }

        [Fact]
        public async Task create_project_command_fails_when_project_with_key_already_exists_in_database()
        {
            var projectId = "key";
            var ownerId = Guid.NewGuid();
            var title = "Title";
            var description = "description";

            // Add owner
            var user = new User(ownerId);
            await _usersMongoDbFixture.InsertAsync(user.AsDocument());

            // Add project
            var project = new Project(projectId, ownerId, null, null, "test.nl/image", title, description, DateTime.UtcNow);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


            var command = new CreateProject(projectId, ownerId, null, null, "test.nl/image", title, description);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<ProjectAlreadyExistsException>();
        }

        [Fact]
        public async Task create_project_command_fails_when_owner_does_not_exist()
        {
            var projectId = "key";
            var ownerId = Guid.NewGuid();
            var key = "key";
            var title = "Title";
            var description = "description";

            var command = new CreateProject(projectId, ownerId, null, null, "test.nl/image", title, description);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<UserNotFoundException>();
        }
    }
}
