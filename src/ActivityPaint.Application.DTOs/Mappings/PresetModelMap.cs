using ActivityPaint.Application.DTOs.Models;
using ActivityPaint.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.DTOs.Mappings;

[Mapper]
public static partial class PresetModelMap
{
    [MapperIgnoreTarget(nameof(Preset.Id))]
    [MapperIgnoreTarget(nameof(Preset.IsDeleted))]
    public static partial Preset ToPreset(this PresetModel presetModel);

    [MapperIgnoreSource(nameof(Preset.Id))]
    [MapperIgnoreSource(nameof(Preset.IsDeleted))]
    public static partial PresetModel ToPresetModel(this Preset preset);
}
