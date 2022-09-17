using System;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Projects.Infrastructure.Mongo.Documents;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Projects.Tests.Integration;

[CollectionDefinition(nameof(SpirebyteCollection), DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<DockerDbFixture>,
    ICollectionFixture<MongoDbFixture<IssueDocument, string>>,
    ICollectionFixture<MongoDbFixture<PermissionSchemeDocument, Guid>>,
    ICollectionFixture<MongoDbFixture<ProjectGroupDocument, Guid>>,
    ICollectionFixture<MongoDbFixture<ProjectDocument, string>>,
    ICollectionFixture<MongoDbFixture<SprintDocument, string>>,
    ICollectionFixture<MongoDbFixture<UserDocument, Guid>>
{
}