using FluentAssertions;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class UserScheduleRepositoryTests
{
    private readonly IUserScheduleRepository _repository;

    public UserScheduleRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.UserScheduleRepository;
    }
    
    [Fact]
    public async Task IsWorkDay_OfficeSchedule_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        const int year = 2023;
        const int month = 9;

        var startMonth = new DateOnly(year, month, 1);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var monthWorkDaysMap = Enumerable.Range(0, daysInMonth)
            .Select(x => startMonth.AddDays(x))
            .Select(x => x.DayOfWeek switch
            {
                DayOfWeek.Sunday => false,
                DayOfWeek.Saturday => false,
                _ => true
            })
            .ToArray();
        
        var schedule = UserScheduleModelFaker.Generate()
            .First()
            .WithUserId(userId)
            .WithYear(year)
            .WithMonth(month)
            .WithScheduleMap(monthWorkDaysMap);
        
        await _repository.Init(schedule, default); 
        
        // Act
        var result = new List<bool>();
        for (var i = 0; i < daysInMonth; i++)
        {
            var date = new DateOnly(year, month, i + 1);
            result.Add(await _repository.IsWorkDay(userId, date,  default));
        }
        
        
        // Assert
        result.Should().BeEquivalentTo(monthWorkDaysMap);
    }
    
    [Fact]
    public async Task TakeDayOff_AllWorkDaysSchedule_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var date = new DateOnly(2023, 09, 27);

        var schedule = UserScheduleModelFaker.Generate()
            .First()
            .WithUserId(userId)
            .WithYear(date.Year)
            .WithMonth(date.Month)
            .WithAllWorkDaysSchedule();
        
        await _repository.Init(schedule, default); 
        
        // Act
        await _repository.TakeDayOff(userId, date, default); 
        
        // Assert
        var result = await _repository.IsWorkDay(userId, date,  default); 
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task TakeExtraDay_AllDaysOffSchedule_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var date = new DateOnly(2023, 09, 27);

        var schedule = UserScheduleModelFaker.Generate()
            .First()
            .WithUserId(userId)
            .WithYear(date.Year)
            .WithMonth(date.Month)
            .WithAllDaysOffSchedule();
        
        await _repository.Init(schedule, default); 
        
        // Act
        await _repository.TakeExtraDay(userId, date, default); 
        
        // Assert
        var result = await _repository.IsWorkDay(userId, date,  default); 
        result.Should().BeTrue();
    }
}