using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Save;

[Mapper]
public static partial class SaveCommandSettingsMap
{
    [MapperIgnoreSource(nameof(SaveCommandSettings.Path))]
    [MapperIgnoreSource(nameof(SaveCommandSettings.Overwrite))]
    [MapperIgnoreSource(nameof(SaveCommandSettings.StartDateString))]
    [MapperIgnoreSource(nameof(SaveCommandSettings.CanvasDataString))]
    public static partial PresetModel ToPresetModel(this SaveCommandSettings model);

    public static SavePresetCommand ToSavePresetCommand(this SaveCommandSettings model) => new(
        Preset: model.ToPresetModel(),
        Path: model.Path,
        Overwrite: model.Overwrite
    );
}
