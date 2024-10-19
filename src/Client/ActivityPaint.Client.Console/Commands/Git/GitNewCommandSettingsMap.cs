using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Git;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GitNewCommandSettingsMap
{
    [MapValue(nameof(PresetModel.IsDarkModeDefault), false)]
    public static partial PresetModel ToPresetModel(this GitNewCommandSettings model);

    [MapPropertyFromSource(nameof(GenerateGitCmdCommand.Preset), Use = nameof(ToPresetModel))]
    public static partial GenerateGitCmdCommand ToGenerateGitCmdCommand(this GitNewCommandSettings model);
}
