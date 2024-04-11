namespace HomeworkApp.Dal.Models;

public record UserScheduleModel
{
    public required long UserId { get; init; }

    public required int Year { get; init; }
    
    public required int Month { get; init; }
    
    public required bool[] ScheduleMap { get; init; }
}