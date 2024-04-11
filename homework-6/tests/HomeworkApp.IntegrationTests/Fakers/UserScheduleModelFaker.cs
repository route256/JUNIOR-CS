using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Models;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class UserScheduleModelFaker

{
    private static readonly object Lock = new();

    private static readonly Faker<UserScheduleModel> Faker = new AutoFaker<UserScheduleModel>()
        .RuleFor(x => x.UserId, _ => Create.RandomId())
        .RuleFor(x => x.Month, f => f.Random.Int(1,12))
        .RuleFor(x => x.Year, f => f.Random.Int(2023,2024));

    public static UserScheduleModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }

    public static UserScheduleModel WithUserId(
        this UserScheduleModel src, 
        long userId)
        => src with { UserId = userId };
    
    public static UserScheduleModel WithMonth(
        this UserScheduleModel src, 
        int month)
        => src with { Month = month };

    public static UserScheduleModel WithYear(
        this UserScheduleModel src, 
        int year)
        => src with { Year = year };
    
    public static UserScheduleModel WithScheduleMap(
        this UserScheduleModel src, 
        bool[] scheduleMap)
        => src with { ScheduleMap = scheduleMap };
    
    public static UserScheduleModel WithAllWorkDaysSchedule(
        this UserScheduleModel src)
        => src with { ScheduleMap = Enumerable.Repeat(true , DateTime.DaysInMonth(src.Year, src.Month)).ToArray() };
    
    public static UserScheduleModel WithAllDaysOffSchedule(
        this UserScheduleModel src)
        => src with { ScheduleMap = Enumerable.Repeat(false , DateTime.DaysInMonth(src.Year, src.Month)).ToArray() };
}