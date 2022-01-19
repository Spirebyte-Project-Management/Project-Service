using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Projects.Tests.Integration;

[CollectionDefinition("Spirebyte collection", DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<Program>>
{
}