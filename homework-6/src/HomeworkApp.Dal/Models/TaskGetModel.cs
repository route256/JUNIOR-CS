namespace HomeworkApp.Dal.Models;

public record TaskGetModel
{
    public required long[] TaskIds { get; init; }
}