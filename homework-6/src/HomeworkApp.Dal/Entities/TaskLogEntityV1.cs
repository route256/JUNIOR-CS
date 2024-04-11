namespace HomeworkApp.Dal.Entities;

public record TaskLogEntityV1
{
    public long Id { get; init; }
    
    public required long TaskId { get; init; }
    
    public long? ParentTaskId { get; init; }

    public required string Number { get; init; }
    
    public required string Title { get; init; }
    
    public string? Description { get; init; }
    
    public required int Status { get; init; }
    
    public long? AssignedToUserId { get; init; }
    
    public required long UserId { get; init; }
    
    public required DateTimeOffset At { get; init; }
}
