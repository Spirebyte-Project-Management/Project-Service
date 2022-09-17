using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Projects.Application.Projects.DTO;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;
using Spirebyte.Services.Projects.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Queries;

public class GetProjectsTests : TestBase
{
    private readonly IQueryHandler<GetProjects, IEnumerable<ProjectDto>> _queryHandler;

    private readonly IContextAccessor _contextAccessor;
    private readonly ILogger<GetProjectsHandler> _logger;

    public GetProjectsTests(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(issuesMongoDbFixture, permissionSchemesMongoDbFixture, projectGroupsMongoDbFixture, projectsMongoDbFixture, sprintsMongoDbFixture, usersMongoDbFixture)
    {
        _contextAccessor = Substitute.For<IContextAccessor>();
        _logger = Substitute.For<ILogger<GetProjectsHandler>>();
        
        _queryHandler = new GetProjectsHandler(ProjectsMongoDbFixture, _contextAccessor, _logger);
    }

    [Fact]
    public async Task get_projects_query_succeeds_when_a_project_exists()
    {
        var generatedProjects = ProjectFaker.Instance.Generate(10);
        foreach (var generatedProject in generatedProjects)
        {
            var generatedProjectDocument = generatedProject.AsDocument();
            generatedProjectDocument.OwnerUserId = generatedProjects.First().OwnerUserId;
            await ProjectsMongoDbFixture.AddAsync(generatedProjectDocument);
        }
        
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", generatedProjects.First().OwnerUserId.ToString()));

        var query = new GetProjects();

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        var projectDtos = result as ProjectDto[] ?? result.ToArray();
        projectDtos.Should().BeEquivalentTo(generatedProjects, options => options.Excluding(c => c.CreatedAt).Excluding(c => c.OwnerUserId));
    }

    [Fact]
    public async Task get_projects_query_should_return_empty_when_no_projects_exist()
    {
        var query = new GetProjects();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id",Guid.NewGuid().ToString()));

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeEmpty();
    }
}