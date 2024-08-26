using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Helpers;
using ActivityPaint.Core.Shared.Progress;

namespace ActivityPaint.Client.Console.Commands.Generate;

public static class GenerateNewCommandSettingsMap
{
    public static PresetModel ToPresetModel(this GenerateNewCommandSettings model) => new(
        Name: model.Name ?? string.Empty,
        StartDate: model.StartDate,
        IsDarkModeDefault: false,
        CanvasData: CanvasDataHelper.ConvertToList(model.CanvasData ?? string.Empty)
    );

    public static AuthorModel ToAuthorModel(this GenerateNewCommandSettings model) => new(
        Email: model.AuthorEmail ?? string.Empty,
        FullName: model.AuthorFullName ?? string.Empty
    );

    public static GenerateRepoCommand ToGenerateRepoCommand(this GenerateNewCommandSettings model, Progress? callback = null) => new(
        Preset: model.ToPresetModel(),
        Author: model.ToAuthorModel(),
        Path: model.Path ?? string.Empty,
        MessageFormat: model.MessageFormat,
        ProgressCallback: callback
    );

    public static DownloadRepoCommand ToDownloadRepoCommand(this GenerateNewCommandSettings model, Progress? callback = null) => new(
        Preset: model.ToPresetModel(),
        Author: model.ToAuthorModel(),
        MessageFormat: model.MessageFormat,
        ProgressCallback: callback
    );
}
