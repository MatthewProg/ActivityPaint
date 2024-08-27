using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Progress;

namespace ActivityPaint.Client.Console.Commands.Generate;

public static class GenerateLoadCommandSettingsMap
{
    public static AuthorModel ToAuthorModel(this GenerateLoadCommandSettings model) => new(
        Email: model.AuthorEmail,
        FullName: model.AuthorFullName
    );

    public static GenerateRepoCommand ToGenerateRepoCommand(this GenerateLoadCommandSettings model, PresetModel preset, Progress? callback = null) => new(
        Preset: preset,
        Author: model.ToAuthorModel(),
        Path: model.OutputPath,
        MessageFormat: model.MessageFormat,
        ProgressCallback: callback
    );

    public static DownloadRepoCommand ToDownloadRepoCommand(this GenerateLoadCommandSettings model, PresetModel preset, Progress? callback = null) => new(
        Preset: preset,
        Author: model.ToAuthorModel(),
        MessageFormat: model.MessageFormat,
        ProgressCallback: callback
    );
}
