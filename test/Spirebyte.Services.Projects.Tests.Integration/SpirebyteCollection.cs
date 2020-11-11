using Spirebyte.Services.Projects.API;
using Spirebyte.Services.Projects.Tests.Shared.Factories;
using Xunit;

namespace Spirebyte.Services.Projects.Tests.Integration
{
    [CollectionDefinition("Spirebyte collection")]
    public class SpirebyteCollection : ICollectionFixture<SpirebyteApplicationFactory<Program>>
    {
    }
}