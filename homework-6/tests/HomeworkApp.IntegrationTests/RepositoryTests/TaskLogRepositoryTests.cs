using FluentAssertions;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class TaskLogRepositoryTests
{
    private readonly ITaskLogRepository _repository;

    public TaskLogRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.TaskLogRepository;
    }

    [Fact]
    public async Task Add_TaskLog_Success()
    {
        // Arrange
        const int count = 5;

        var tasks = TaskLogEntityV1Faker.Generate(count);
        
        // Act
        var results = await _repository.Add(tasks, default);

        // Asserts
        results.Should().HaveCount(count);
        results.Should().OnlyContain(x => x > 0);
    }
    
    [Fact]
    public async Task Get_LogsForSingleTask_Success()
    {
        // Arrange
        var taskId = Create.RandomId();
        var anotherTaskId = Create.RandomId();

        var taskLogs = TaskLogEntityV1Faker.Generate(3)
            .Select(x => x.WithTaskId(taskId))
            .ToArray();
        
        var anotherTaskLogs = TaskLogEntityV1Faker.Generate(3)
            .Select(x => x.WithTaskId(anotherTaskId))
            .ToArray();
        
        var logs = taskLogs
            .Union(anotherTaskLogs)
            .ToArray();
        
        await _repository.Add(logs, default);
        
        // Act
        var results = await _repository.Get(new TaskLogGetModel()
        {
            TaskIds = new[] { taskId }
        }, default);
        
        // Asserts
        results.Should().NotBeEmpty();
        
        results.Should().BeEquivalentTo(taskLogs, 
            opt => opt.Excluding(ctx => ctx.Id));
    }
}
