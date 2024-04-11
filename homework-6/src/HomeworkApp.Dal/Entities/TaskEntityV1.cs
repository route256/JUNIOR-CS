namespace HomeworkApp.Dal.Entities;

public record TaskEntityV1
{
    public long Id { get; init; }
    
    public long? ParentTaskId { get; init; }

    public required string Number { get; init; }
    
    public required string Title { get; init; }
    
    public string? Description { get; init; }
    
    public required int Status { get; init; }
    
    public required DateTimeOffset CreatedAt { get; init; }
    
    public required long CreatedByUserId { get; init; }
    
    public long? AssignedToUserId { get; init; }
    
    public DateTimeOffset? CompletedAt { get; init; }
}
