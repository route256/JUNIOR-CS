using TaskStatus = HomeworkApp.Bll.Enums.TaskStatus;

namespace HomeworkApp.Bll.Models;

public record GetTaskModel
{
    public long TaskId { get; init; }
    
    public long? ParentTaskId { get; init; }

    public required string Number { get; init; }
    
    public required string Title { get; init; }
    
    public string? Description { get; init; }
    
    public required TaskStatus Status { get; init; }
    
    public required DateTimeOffset CreatedAt { get; init; }
    
    public required long CreatedByUserId { get; init; }
    
    public long? AssignedToUserId { get; init; }
    
    public DateTimeOffset? CompletedAt { get; init; }
}