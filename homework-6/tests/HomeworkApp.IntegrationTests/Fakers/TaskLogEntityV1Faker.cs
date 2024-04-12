using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Entities;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class TaskLogEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<TaskLogEntityV1> Faker = new AutoFaker<TaskLogEntityV1>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.Status, f => f.Random.Int(1, 5))
        .RuleFor(x => x.UserId, _ => Create.RandomId())
        .RuleFor(x => x.AssignedToUserId, _ => Create.RandomId())
        .RuleFor(x => x.ParentTaskId, _ => Create.RandomId())
        .RuleFor(x => x.At, f => f.Date.RecentOffset().UtcDateTime);

    public static TaskLogEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static TaskLogEntityV1 WithTaskId(
        this TaskLogEntityV1 src, 
        long taskId)
        => src with { TaskId = taskId };

    public static TaskLogEntityV1 WithId(
        this TaskLogEntityV1 src, 
        long id)
        => src with { Id = id };
    
}