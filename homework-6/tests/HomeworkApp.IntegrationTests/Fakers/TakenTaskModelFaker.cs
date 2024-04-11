using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Models;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class TakenTaskModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<TakenTaskModel> Faker = new AutoFaker<TakenTaskModel>()
        .RuleFor(x => x.TaskId, _ => Create.RandomId())
        .RuleFor(x => x.AssignedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static TakenTaskModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static TakenTaskModel WithTaskId(
        this TakenTaskModel src, 
        long taskId)
        => src with { TaskId = taskId };
    
}