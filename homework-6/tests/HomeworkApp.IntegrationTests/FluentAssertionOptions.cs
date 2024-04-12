using FluentAssertions;

namespace HomeworkApp.IntegrationTests;

public static class FluentAssertionOptions
{
    private static readonly TimeSpan RequiredDateTimePrecision = TimeSpan.FromMilliseconds(100);

    public static void UseDefaultPrecision()
    {
        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(
                ctx => ctx.Subject.Should()
                    .BeCloseTo(ctx.Expectation, RequiredDateTimePrecision))
                .WhenTypeIs<DateTime>();
            
            options.Using<DateTimeOffset>(
                ctx => ctx.Subject.Should()
                    .BeCloseTo(ctx.Expectation, RequiredDateTimePrecision))
                .WhenTypeIs<DateTimeOffset>();
            
            return options;
        });
    }
}