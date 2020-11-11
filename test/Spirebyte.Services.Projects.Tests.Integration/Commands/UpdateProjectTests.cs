﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.Commands;
using Spirebyte.Services.Projects.Application.Exceptions;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Base;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Commands
{
    [Collection("Spirebyte collection")]
    public class UpdateProjectTests : IDisposable
    {
        public UpdateProjectTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _projectsMongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            _usersMongoDbFixture = new MongoDbFixture<UserDocument, Guid>("users");
            factory.Server.AllowSynchronousIO = true;
            _commandHandler = factory.Services.GetRequiredService<ICommandHandler<UpdateProject>>();
        }

        public void Dispose()
        {
            _projectsMongoDbFixture.Dispose();
            _usersMongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, Guid> _projectsMongoDbFixture;
        private readonly MongoDbFixture<UserDocument, Guid> _usersMongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly ICommandHandler<UpdateProject> _commandHandler;


        [Fact]
        public async Task update_project_command_should_modify_project_with_given_data()
        {
            var projectId = new AggregateId();
            var ownerId = Guid.NewGuid();
            var key = "key";
            var title = "Title";
            var updatedTitle = "UpdatedTitle";
            var description = "description";
            var updatedDescription = "UpdatedDescription";

            var user = new User(ownerId);
            await _usersMongoDbFixture.InsertAsync(user.AsDocument());

            var project = new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);
            await _projectsMongoDbFixture.InsertAsync(project.AsDocument());


            var command = new UpdateProject(projectId, key,null, null, "test.nl/image", null, updatedTitle, updatedDescription);

            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().NotThrow();


            var updatedProject = await _projectsMongoDbFixture.GetAsync(command.ProjectId);

            updatedProject.Should().NotBeNull();
            updatedProject.Id.Should().Be(projectId);
            updatedProject.OwnerUserId.Should().Be(ownerId);
            updatedProject.Title.Should().Be(updatedTitle);
            updatedProject.Description.Should().Be(updatedDescription);
        }

        [Fact]
        public async Task update_project_command_fails_when_project_does_not_exists_in_database()
        {
            var projectId = new AggregateId();
            var ownerId = Guid.NewGuid();
            var key = "key";
            var title = "Title";
            var updatedTitle = "UpdatedTitle";
            var description = "description";
            var updatedDescription = "UpdatedDescription";

            // Add owner
            var user = new User(ownerId);
            await _usersMongoDbFixture.InsertAsync(user.AsDocument());

            var command = new UpdateProject(projectId, key,null, null, "test.nl/image", null, updatedTitle, updatedDescription);


            // Check if exception is thrown

            _commandHandler
                .Awaiting(c => c.HandleAsync(command))
                .Should().Throw<ProjectNotFoundException>();
        }
    }
}
