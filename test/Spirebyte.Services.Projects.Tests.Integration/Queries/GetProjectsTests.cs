using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Application.DTO;
using Spirebyte.Services.Projects.Application.Queries;
using Spirebyte.Services.Projects.Core.Entities;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Spirebyte.Services.Projects.Tests.Shared.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Queries
{
    [Collection("Spirebyte collection")]
    public class GetProjectsTests : IDisposable
    {
        public GetProjectsTests(SpirebyteApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture();
            _mongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
            factory.Server.AllowSynchronousIO = true;
            _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetProjects, IEnumerable<ProjectDto>>>();
        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }

        private const string Exchange = "projects";
        private readonly MongoDbFixture<ProjectDocument, string> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly IQueryHandler<GetProjects, IEnumerable<ProjectDto>> _queryHandler;


        [Fact]
        public async Task get_projects_query_succeeds_when_a_project_exists()
        {
            var projectId = "key" + Guid.NewGuid();
            var projectId2 = "key" + Guid.NewGuid();
            var permissionSchemeId = 1;
            var ownerId = Guid.NewGuid();
            var title = "Title";
            var description = "description";

            var project = new Project(projectId, permissionSchemeId, ownerId, null, null, "test.nl/image", title, description, 0, DateTime.UtcNow);
            var project2 = new Project(projectId2, permissionSchemeId, ownerId, null, null, "test.nl/image", title, description, 0, DateTime.UtcNow);
            await _mongoDbFixture.InsertAsync(project.AsDocument());
            await _mongoDbFixture.InsertAsync(project2.AsDocument());


            var query = new GetProjects();

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            var projectDtos = result as ProjectDto[] ?? result.ToArray();
            projectDtos.Should().Contain(i => i.Id == projectId);
            projectDtos.Should().Contain(i => i.Id == projectId2);
        }

        [Fact]
        public async Task get_projects_query_should_return_empty_when_no_projects_exist()
        {
            var query = new GetProjects();

            // Check if exception is thrown

            var requestResult = _queryHandler
                .Awaiting(c => c.HandleAsync(query));

            requestResult.Should().NotThrow();

            var result = await requestResult();

            result.Should().BeEmpty();
        }
    }
}
