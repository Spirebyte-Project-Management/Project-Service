using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
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
    public class DoesKeyExistTests : IDisposable
    {
        public DoesKeyExistTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<ProjectDocument, Guid>("projects");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<DoesKeyExist, bool>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IQueryHandler<DoesKeyExist, bool> _queryHandler;


        [Fact]
        public async Task does_key_exist_query_returns_true_when_project_with_key_exist()
        {
            var projectId = new AggregateId();
            var ownerId = new AggregateId();
            var key = "key";
            var title = "Title";
            var description = "description";

            var project = new Project(projectId, ownerId, null, null, key, "test.nl/image", title, description, DateTime.UtcNow);
            await _mongoDbFixture.InsertAsync(project.AsDocument());


            var query = new DoesKeyExist(key);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeTrue();
        }

        [Fact]
        public async Task does_key_exist_query_returns_false_when_np_project_with_key_exist()
        {
            var key = "notExistingKey";

            var query = new DoesKeyExist(key);

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeFalse();
        }
    }
}
