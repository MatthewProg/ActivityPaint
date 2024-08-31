using ActivityPaint.Application.BusinessLogic.Preset.Models;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.BusinessLogic.Preset.Mappers;

[Mapper]
internal static partial class PresetFileModelMap
{
    public static partial PresetModel ToPresetModel(this PresetFileModel presetModel);

    [MapperIgnoreSource(nameof(PresetModel.CanvasDataString))]
    public static partial PresetFileModel ToPresetFileModel(this PresetModel preset);
}
