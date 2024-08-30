using ActivityPaint.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.DTOs.Repository;

[Mapper]
public static partial class AuthorModelMap
{
    [MapperIgnoreSource(nameof(RepositoryConfig.Id))]
    [MapperIgnoreSource(nameof(RepositoryConfig.MessageFormat))]
    [MapProperty(nameof(RepositoryConfig.AuthorEmail), nameof(AuthorModel.Email))]
    [MapProperty(nameof(RepositoryConfig.AuthorFullName), nameof(AuthorModel.FullName))]
    public static partial AuthorModel ToAuthorModel(this RepositoryConfig model);

    [MapProperty(nameof(AuthorModel.Email), nameof(RepositoryConfig.AuthorEmail))]
    [MapProperty(nameof(AuthorModel.FullName), nameof(RepositoryConfig.AuthorFullName))]
    public static partial void ToRepositoryConfig(AuthorModel from, RepositoryConfig to);
}
