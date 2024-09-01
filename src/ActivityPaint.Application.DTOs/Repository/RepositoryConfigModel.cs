namespace ActivityPaint.Application.DTOs.Repository;

public record RepositoryConfigModel(
    string? MessageFormat,
    string? AuthorEmail,
    string? AuthorFullName);
