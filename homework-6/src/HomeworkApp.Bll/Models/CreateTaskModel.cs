namespace HomeworkApp.Bll.Models;

public record CreateTaskModel
{
    public required string Title { get; init; }
    
    public string? Description { get; init; }
    
    public long UserId { get; init; }
}