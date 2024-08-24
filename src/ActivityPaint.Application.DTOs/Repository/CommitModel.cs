namespace ActivityPaint.Application.DTOs.Repository;

public record CommitModel(
    string Message,
    DateTimeOffset DateTime);
