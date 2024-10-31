using Riok.Mapperly.Abstractions;
using PresetEntity = ActivityPaint.Core.Entities.Preset;

namespace ActivityPaint.Application.DTOs.Preset;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class PresetModelMap
{
    [MapperIgnoreTarget(nameof(PresetEntity.Id))]
    [MapValue(nameof(PresetEntity.LastUpdated), Use = nameof(GetUtc))]
    public static partial PresetEntity ToPreset(this PresetModel presetModel);

    public static partial PresetModel ToPresetModel(this PresetEntity preset);

    private static DateTimeOffset GetUtc() => DateTimeOffset.UtcNow;
}
