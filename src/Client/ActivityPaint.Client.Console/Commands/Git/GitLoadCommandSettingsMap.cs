using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Git;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GitLoadCommandSettingsMap
{
    public static partial GenerateGitCmdCommand ToGenerateGitCmdCommand(this GitLoadCommandSettings model, PresetModel preset);
}
