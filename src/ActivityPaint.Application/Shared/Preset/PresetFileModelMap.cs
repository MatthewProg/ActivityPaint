using ActivityPaint.Application.DTOs.Models;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Application.BusinessLogic.Shared.Preset;

[Mapper]
internal static partial class PresetFileModelMap
{
    public static partial PresetModel ToPresetModel(this PresetFileModel presetModel);

    [MapperIgnoreSource(nameof(PresetModel.CanvasDataString))]
    public static partial PresetFileModel ToPresetFileModel(this PresetModel preset);
}
