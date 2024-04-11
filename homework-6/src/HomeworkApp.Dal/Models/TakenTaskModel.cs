namespace HomeworkApp.Dal.Models;

public record TakenTaskModel
{
    public long TaskId { get; init; }
    
    public string? Title { get; init; }
    
    public long AssignedToUserId { get; init; }

    public string AssignedToEmail { get; init; } = string.Empty;
    
    public DateTimeOffset AssignedAt { get; init; }
}