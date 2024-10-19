using ActivityPaint.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.DTOs.Repository;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RepositoryConfigModelMap
{
    [MapperIgnoreTarget(nameof(RepositoryConfig.Id))]
    public static partial RepositoryConfig ToRepositoryConfig(this RepositoryConfigModel model);

    public static partial RepositoryConfigModel ToRepositoryConfigModel(this RepositoryConfig model);
}
