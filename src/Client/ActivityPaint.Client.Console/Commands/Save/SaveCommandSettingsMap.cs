using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Save;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class SaveCommandSettingsMap
{
    public static partial PresetModel ToPresetModel(this SaveCommandSettings model);

    [MapPropertyFromSource(nameof(SavePresetCommand.Preset), Use = nameof(ToPresetModel))]
    public static partial SavePresetCommand ToSavePresetCommand(this SaveCommandSettings model);
}
