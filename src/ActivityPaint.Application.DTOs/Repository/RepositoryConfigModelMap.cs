
using ActivityPaint.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.DTOs.Repository;

[Mapper]
public static partial class RepositoryConfigModelMap
{
    [MapperIgnoreTarget(nameof(RepositoryConfig.Id))]
    public static partial RepositoryConfig ToRepositoryConfig(this RepositoryConfigModel model);

    [MapperIgnoreSource(nameof(RepositoryConfig.Id))]
    public static partial RepositoryConfigModel ToRepositoryConfigModel(this RepositoryConfig model);
}
