using ActivityPaint.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.DTOs.Repository;

[Mapper]
public static partial class AuthorModelMap
{
    public static AuthorModel ToAuthorModel(this RepositoryConfig model) => new(
        FullName: model.AuthorFullName ?? "Activity Paint",
        Email: model.AuthorEmail ?? "email@example.com"
    );

    [MapProperty(nameof(AuthorModel.Email), nameof(RepositoryConfig.AuthorEmail))]
    [MapProperty(nameof(AuthorModel.FullName), nameof(RepositoryConfig.AuthorFullName))]
    public static partial void ToRepositoryConfig(AuthorModel from, RepositoryConfig to);
}
