using System;
using System.Threading.Tasks;
using FluentAssertions;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Projects.Application.Projects.Queries;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Queries.Handler;
using Spirebyte.Services.Projects.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration.Queries;

public class DoesProjectExistTests : TestBase
{
    private readonly IQueryHandler<DoesProjectExist, bool> _queryHandler;

    public DoesProjectExistTests(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture) : base(issuesMongoDbFixture, permissionSchemesMongoDbFixture, projectGroupsMongoDbFixture, projectsMongoDbFixture, sprintsMongoDbFixture, usersMongoDbFixture)
    {
        _queryHandler = new DoesProjectExistHandler(ProjectsMongoDbFixture);
    }

    [Fact]
    public async Task does_project_exist_query_returns_true_when_project_with_id_exist()
    {
        var fakedProject = ProjectFaker.Instance.Generate();
        await ProjectsMongoDbFixture.AddAsync(fakedProject.AsDocument());


        var query = new DoesProjectExist(fakedProject.Id);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task does_project_exist_query_returns_false_when_no_project_with_id_exist()
    {
        var fakedProject = ProjectFaker.Instance.Generate();
        
        var query = new DoesProjectExist(fakedProject.Id);

        // Check if exception is thrown
        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeFalse();
    }
}