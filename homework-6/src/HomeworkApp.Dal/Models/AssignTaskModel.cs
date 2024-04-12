namespace HomeworkApp.Dal.Models;

public record AssignTaskModel
{
    public required long TaskId { get; init; }
    
    public required long AssignToUserId { get; init; }
    
    public required int Status { get; init; }
}