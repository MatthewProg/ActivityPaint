using ActivityPaint.Application.BusinessLogic.Preset.Models;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.BusinessLogic.Preset.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class PresetFileModelMap
{
    public static partial PresetModel ToPresetModel(this PresetFileModel presetModel);

    public static partial PresetFileModel ToPresetFileModel(this PresetModel preset);
}
