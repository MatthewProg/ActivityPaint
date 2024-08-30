using Riok.Mapperly.Abstractions;
using PresetEntity = ActivityPaint.Core.Entities.Preset;

namespace ActivityPaint.Application.DTOs.Preset;

[Mapper]
public static partial class PresetModelMap
{
    [MapperIgnoreTarget(nameof(PresetEntity.Id))]
    [MapperIgnoreSource(nameof(PresetModel.CanvasDataString))]
    public static partial PresetEntity ToPreset(this PresetModel presetModel);

    [MapperIgnoreSource(nameof(PresetEntity.Id))]
    public static partial PresetModel ToPresetModel(this PresetEntity preset);
}
