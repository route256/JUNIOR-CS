using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Entities;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class UserEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<UserEntityV1> Faker = new AutoFaker<UserEntityV1>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.Id, f => f.Random.Long(0L))
        .RuleFor(x => x.Email, f => $"{f.Random.Word()}@test.ru")
        .RuleFor(x => x.CreatedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleFor(x => x.BlockedAt, f => f.Date.RecentOffset().UtcDateTime);

    public static UserEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static UserEntityV1 WithId(
        this UserEntityV1 src, 
        long id)
        => src with { Id = id };
    
    public static UserEntityV1 WithEmail(
        this UserEntityV1 src, 
        string email) 
        => src with { Email = email };

    public static UserEntityV1 WithBlockedAt(
        this UserEntityV1 src, 
        DateTimeOffset? blockedAt)
        => src with { BlockedAt = blockedAt };
}