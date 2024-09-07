using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Client.Console.Commands.Git;

public static class GitNewCommandSettingsMap
{
    public static PresetModel ToPresetModel(this GitNewCommandSettings model) => new(
        Name: model.Name,
        StartDate: model.StartDate,
        IsDarkModeDefault: false,
        CanvasData: model.CanvasData
    );

    public static GenerateGitCmdCommand ToGenerateGitCmdCommand(this GitNewCommandSettings model) => new(
        Preset: model.ToPresetModel(),
        MessageFormat: model.MessageFormat
    );
}
