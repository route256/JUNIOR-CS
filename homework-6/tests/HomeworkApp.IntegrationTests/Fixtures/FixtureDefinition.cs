using Xunit;

namespace HomeworkApp.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(TestFixture))]
    public class FixtureDefinition : ICollectionFixture<TestFixture>
    {
    }
}
