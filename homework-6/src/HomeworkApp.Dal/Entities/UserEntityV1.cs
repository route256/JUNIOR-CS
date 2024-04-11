namespace HomeworkApp.Dal.Entities;

public record UserEntityV1
{
    public required long Id { get; init; }

    public required string Email { get; init; }
    
    public required DateTimeOffset CreatedAt { get; init; }
    
    public DateTimeOffset? BlockedAt { get; init; }
}
