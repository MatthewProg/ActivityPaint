namespace ActivityPaint.Core.Entities;

public sealed class RepositoryConfig : BaseEntity
{
    public string? MessageFormat { get; set; }
    public string? AuthorEmail { get; set; }
    public string? AuthorFullName { get; set; }
}
