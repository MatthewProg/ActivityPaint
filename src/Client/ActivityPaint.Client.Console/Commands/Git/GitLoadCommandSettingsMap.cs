using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Client.Console.Commands.Git;

public static class GitLoadCommandSettingsMap
{
    public static GenerateGitCmdCommand ToGenerateGitCmdCommand(this GitLoadCommandSettings model, PresetModel preset) => new(
        Preset: preset,
        MessageFormat: model.MessageFormat
    );
}
