using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class ProjectHasUserTests : IDisposable
    {
        public ProjectHasUserTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<ProjectHasUser, bool>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, string> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IQueryHandler<ProjectHasUser, bool> _queryHandler;


        [Fact]
        public async Task project_has_user_query_returns_true_when_project_owner_is_given_user_and_project_exists()
        {
            var projectId = "key";
            var ownerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var title = "Title";
            var description = "description";

            var project = new Project(projectId, ownerId, new[] { userId }, null, "test.nl/image", title, description, 0, DateTime.UtcNow);
            await _mongoDbFixture.InsertAsync(project.AsDocument());


            var query = new ProjectHasUser(projectId, ownerId);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeTrue();
        }

        [Fact]
        public async Task project_has_user_query_returns_true_when_project_members_contain_given_user_and_project_exists()
        {
            var projectId = "key";
            var ownerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var title = "Title";
            var description = "description";

            var project = new Project(projectId, ownerId, new[] { userId }, null, "test.nl/image", title, description, 0, DateTime.UtcNow);
            await _mongoDbFixture.InsertAsync(project.AsDocument());


            var query = new ProjectHasUser(projectId, userId);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeTrue();
        }

        [Fact]
        public async Task project_has_user_query_returns_false_when_project_does_not_contain_given_user_and_project_exists()
        {
            var projectId = "key";
            var ownerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var notExistingUserId = Guid.NewGuid();
            var title = "Title";
            var description = "description";

            var project = new Project(projectId, ownerId, new[] { userId }, null, "test.nl/image", title, description, 0, DateTime.UtcNow);
            await _mongoDbFixture.InsertAsync(project.AsDocument());


            var query = new ProjectHasUser(projectId, notExistingUserId);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeFalse();
        }

        [Fact]
        public async Task project_has_user_query_returns_false_when_project_does_not_exists()
        {
            var userId = Guid.NewGuid();
            var key = "key";


            var query = new ProjectHasUser(key, userId);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeFalse();
        }
    }
}
