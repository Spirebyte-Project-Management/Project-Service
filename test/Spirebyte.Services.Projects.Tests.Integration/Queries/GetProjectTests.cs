using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Core.Entities.Base;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetProjectTests : IDisposable
    {
        public GetProjectTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetProject, ProjectDto>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IQueryHandler<GetProject, ProjectDto> _queryHandler;


        [Fact]
        public async Task get_project_query_succeeds_when_project_exists()
        {
            var projectId = new AggregateId();
            var ownerId = new AggregateId();
            var key = "key";
            var title = "Title";
            var description = "description";

            var project = new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);
            await _mongoDbFixture.InsertAsync(project.AsDocument());


            var query = new GetProject(key);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().NotBeNull();
            result.Id.Should().Be(projectId);
            result.OwnerUserId.Should().Be(ownerId);
            result.Title.Should().Be(title);
            result.Description.Should().Be(description);
        }

        [Fact]
        public async Task get_project_query_should_return_null_when_project_does_not_exist()
        {
            var key = "notExistingKey";

            var query = new GetProject(key);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeNull();
        }
    }
}
