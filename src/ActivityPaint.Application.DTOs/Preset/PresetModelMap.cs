using Riok.Mapperly.Abstractions;
using PresetEntity = ActivityPaint.Core.Entities.Preset;

namespace ActivityPaint.Application.DTOs.Preset;

[Mapper]
public static partial class PresetModelMap
{
    [MapperIgnoreTarget(nameof(PresetEntity.Id))]
    [MapperIgnoreTarget(nameof(PresetEntity.IsDeleted))]
    [MapperIgnoreSource(nameof(PresetModel.CanvasDataString))]
    public static partial PresetEntity ToPreset(this PresetModel presetModel);

    [MapperIgnoreSource(nameof(PresetEntity.Id))]
    [MapperIgnoreSource(nameof(PresetEntity.IsDeleted))]
    public static partial PresetModel ToPresetModel(this PresetEntity preset);
}
