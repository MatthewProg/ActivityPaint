using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.DTOs.Models;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Save;

[Mapper]
public static partial class SaveCommandSettingsMap
{
    [MapperIgnoreSource(nameof(SaveCommandSettings.Path))]
    [MapperIgnoreSource(nameof(SaveCommandSettings.Overwrite))]
    [MapperIgnoreSource(nameof(SaveCommandSettings.StartDateString))]
    [MapProperty(nameof(SaveCommandSettings.CanvasData), nameof(PresetModel.CanvasData), Use = nameof(CanvasDataStringToList))]
    public static partial PresetModel ToPresetModel(this SaveCommandSettings model);

    public static SavePresetCommand ToSavePresetCommand(this SaveCommandSettings model) => new(
        Preset: model.ToPresetModel(),
        Path: model.Path,
        Overwrite: model.Overwrite
    );

    [UserMapping]
    private static List<IntensityEnum> CanvasDataStringToList(string canvasData)
        => CanvasDataHelper.ConvertToList(canvasData);
}
