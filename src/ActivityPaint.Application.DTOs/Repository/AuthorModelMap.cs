using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.DTOs.Repository;

[Mapper]
public static partial class AuthorModelMap
{
    public static AuthorModel ToAuthorModel(this RepositoryConfigModel model) => new(
        FullName: model.AuthorFullName ?? "Activity Paint",
        Email: model.AuthorEmail ?? "email@example.com"
    );
}
