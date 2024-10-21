using ActivityPaint.Application.BusinessLogic.Image;
using ActivityPaint.Application.DTOs.Preset;
using Riok.Mapperly.Abstractions;

namespace ActivityPaint.Client.Console.Commands.Preview;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class PreviewSaveCommandSettingsMap
{
    [MapProperty(nameof(PreviewSaveCommandSettings.OutputPath), nameof(SavePreviewImageCommand.Path))]
    public static partial SavePreviewImageCommand ToSaveCommand(this PreviewSaveCommandSettings settings, PresetModel preset);
}
