using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Progress;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Generate;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GenerateNewCommandSettingsMap
{
    [MapValue(nameof(PresetModel.IsDarkModeDefault), false)]
    public static partial PresetModel ToPresetModel(this GenerateNewCommandSettings model);

    [MapProperty(nameof(GenerateLoadCommandSettings.AuthorEmail), nameof(AuthorModel.Email))]
    [MapProperty(nameof(GenerateLoadCommandSettings.AuthorFullName), nameof(AuthorModel.FullName))]
    public static partial AuthorModel ToAuthorModel(this GenerateNewCommandSettings model);

    [MapPropertyFromSource(nameof(GenerateRepoCommand.Preset), Use = nameof(ToPresetModel))]
    [MapPropertyFromSource(nameof(GenerateRepoCommand.Author), Use = nameof(ToAuthorModel))]
    [MapProperty(nameof(GenerateLoadCommandSettings.ZipMode), nameof(GenerateRepoCommand.Zip))]
    [MapProperty(nameof(GenerateLoadCommandSettings.OutputPath), nameof(GenerateRepoCommand.Path))]
    public static partial GenerateRepoCommand ToGenerateRepoCommand(this GenerateNewCommandSettings model, Progress? progressCallback = null);
}
