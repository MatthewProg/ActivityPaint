using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Progress;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Generate;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GenerateLoadCommandSettingsMap
{
    [MapProperty(nameof(GenerateLoadCommandSettings.AuthorEmail), nameof(AuthorModel.Email))]
    [MapProperty(nameof(GenerateLoadCommandSettings.AuthorFullName), nameof(AuthorModel.FullName))]
    public static partial AuthorModel ToAuthorModel(this GenerateLoadCommandSettings model);

    [MapPropertyFromSource(nameof(GenerateRepoCommand.Author), Use = nameof(ToAuthorModel))]
    [MapProperty(nameof(GenerateLoadCommandSettings.ZipMode), nameof(GenerateRepoCommand.Zip))]
    [MapProperty(nameof(GenerateLoadCommandSettings.OutputPath), nameof(GenerateRepoCommand.Path))]
    public static partial GenerateRepoCommand ToGenerateRepoCommand(this GenerateLoadCommandSettings model, PresetModel preset, Progress? progressCallback = null);
}
