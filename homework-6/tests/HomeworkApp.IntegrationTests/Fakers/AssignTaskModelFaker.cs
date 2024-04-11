using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Models;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class AssignTaskModelFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<AssignTaskModel> Faker = new AutoFaker<AssignTaskModel>()
        .RuleFor(x => x.TaskId, _ => Create.RandomId())
        .RuleFor(x => x.AssignToUserId, _ => Create.RandomId())
        .RuleFor(x => x.Status, f => f.Random.Int(1, 5));

    public static AssignTaskModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static AssignTaskModel WithTaskId(
        this AssignTaskModel src, 
        long taskId)
        => src with { TaskId = taskId };
    
    public static AssignTaskModel WithAssignToUserId(
        this AssignTaskModel src, 
        long assignToUserId)
        => src with { AssignToUserId = assignToUserId };    
}