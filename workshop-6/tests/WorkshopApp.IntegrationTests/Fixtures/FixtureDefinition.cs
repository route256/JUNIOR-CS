using Xunit;

namespace WorkshopApp.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(TestFixture))]
    public class FixtureDefinition : ICollectionFixture<TestFixture>
    {
    }
}
