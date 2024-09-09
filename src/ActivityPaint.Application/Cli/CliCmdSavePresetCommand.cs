using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Cli;

public sealed record CliCmdSavePresetCommand(
    PresetModel Preset
) : IResultRequest<string>;

internal class CliCmdSavePresetCommandValidator : AbstractValidator<CliCmdSavePresetCommand>
{
    public CliCmdSavePresetCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class CliCmdSavePresetCommandHandler : IResultRequestHandler<CliCmdSavePresetCommand, string>
{
    public ValueTask<Result<string>> Handle(CliCmdSavePresetCommand request, CancellationToken cancellationToken)
    {
        var cmdBase = "ap-cli.exe save";

        var optName = $"--name \"{request.Preset.Name}\"";
        var optStartDate = $"--start-date {request.Preset.StartDate:yyyy-MM-dd}";
        var optData = $"--data {request.Preset.CanvasDataString}";
        var optDarkMode = request.Preset.IsDarkModeDefault ? "--dark-mode" : string.Empty;
        var optOutput = $"--output \"{request.Preset.Name}.json\"";

        var cmdElements = new string[] { cmdBase, optName, optStartDate, optData, optDarkMode, optOutput };
        var output = string.Join(' ', cmdElements.Where(x => x != string.Empty));

        return ValueTask.FromResult<Result<string>>(output);
    }
}
