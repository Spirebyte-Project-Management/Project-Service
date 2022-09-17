using System;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration;

[Collection(nameof(SpirebyteCollection))]
public class TestBase : IDisposable
{
    protected readonly MongoDbFixture<IssueDocument, string> IssuesMongoDbFixture;
    protected readonly MongoDbFixture<PermissionSchemeDocument, Guid> PermissionSchemesMongoDbFixture;
    protected readonly MongoDbFixture<ProjectGroupDocument, Guid> ProjectGroupsMongoDbFixture;
    protected readonly MongoDbFixture<ProjectDocument, string> ProjectsMongoDbFixture;
    protected readonly MongoDbFixture<SprintDocument, string> SprintsMongoDbFixture;
    protected readonly MongoDbFixture<UserDocument, Guid> UsersMongoDbFixture;

    public TestBase(
        MongoDbFixture<IssueDocument, string> issuesMongoDbFixture,
        MongoDbFixture<PermissionSchemeDocument, Guid> permissionSchemesMongoDbFixture,
        MongoDbFixture<ProjectGroupDocument, Guid> projectGroupsMongoDbFixture,
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<SprintDocument, string> sprintsMongoDbFixture,
        MongoDbFixture<UserDocument, Guid> usersMongoDbFixture)
    {
        IssuesMongoDbFixture = issuesMongoDbFixture;
        PermissionSchemesMongoDbFixture = permissionSchemesMongoDbFixture;
        ProjectGroupsMongoDbFixture = projectGroupsMongoDbFixture;
        ProjectsMongoDbFixture = projectsMongoDbFixture;
        SprintsMongoDbFixture = sprintsMongoDbFixture;
        UsersMongoDbFixture = usersMongoDbFixture;
    }
    
    public void Dispose()
    {
        IssuesMongoDbFixture.Dispose();
        PermissionSchemesMongoDbFixture.Dispose();
        ProjectGroupsMongoDbFixture.Dispose();
        ProjectsMongoDbFixture.Dispose();
        SprintsMongoDbFixture.Dispose();
        UsersMongoDbFixture.Dispose();
    }
}