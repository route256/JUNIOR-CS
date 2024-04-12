using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Entities;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class TaskEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<TaskEntityV1> Faker = new AutoFaker<TaskEntityV1>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.Status, f => f.Random.Int(1, 5))
        .RuleFor(x => x.CreatedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleFor(x => x.CompletedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static TaskEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static TaskEntityV1 WithCreatedByUserId(
        this TaskEntityV1 src, 
        long userId)
        => src with { CreatedByUserId = userId };
    
    public static TaskEntityV1 WithId(
        this TaskEntityV1 src, 
        long id)
        => src with { Id = id };
    
    public static TaskEntityV1 WithAssignedToUserId(
        this TaskEntityV1 src, 
        long assignedToUserId)
        => src with { AssignedToUserId = assignedToUserId };
}