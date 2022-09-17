using System;
using System.Threading.Tasks;
using FluentAssertions;
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

public class GetProjectTests : TestBase
{
    private readonly IQueryHandler<GetProject, ProjectDto> _queryHandler;

    public GetProjectTests(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(issuesMongoDbFixture, permissionSchemesMongoDbFixture, projectGroupsMongoDbFixture, projectsMongoDbFixture, sprintsMongoDbFixture, usersMongoDbFixture)
    {
        _queryHandler = new GetProjectHandler(ProjectsMongoDbFixture);
    }

    [Fact]
    public async Task get_project_query_succeeds_when_project_with_id_exists()
    {
        var fakedProject = ProjectFaker.Instance.Generate();
        await ProjectsMongoDbFixture.AddAsync(fakedProject.AsDocument());
        
        var query = new GetProject(fakedProject.Id);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().NotBeNull();
        result.Id.Should().Be(fakedProject.Id);
        result.OwnerUserId.Should().Be(fakedProject.OwnerUserId);
        result.Title.Should().Be(fakedProject.Title);
        result.Description.Should().Be(fakedProject.Description);
    }

    [Fact]
    public async Task get_project_query_should_return_null_when_project_with_id_does_not_exist()
    {
        var projectId = "notExistingKey";

        var query = new GetProject(projectId);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeNull();
    }
}