namespace ActivityPaint.Application.Abstractions.Repository.Models;

public record CommitModel(
    string Message,
    DateTimeOffset DateTime);
