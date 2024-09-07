using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Cli;

public sealed record CliCmdGenerateGitCommand(
    PresetModel Preset,
    string? MessageFormat = null
) : IResultRequest<string>;

internal class CliCmdGenerateGitCommandValidator : AbstractValidator<CliCmdGenerateGitCommand>
{
    public CliCmdGenerateGitCommandValidator(
        IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class CliCmdGenerateGitCommandHandler : IResultRequestHandler<CliCmdGenerateGitCommand, string>
{
    public ValueTask<Result<string>> Handle(CliCmdGenerateGitCommand request, CancellationToken cancellationToken)
    {
        var cmdBase = "ap-cli.exe git";

        var optMessageFormat = request.MessageFormat is null ? string.Empty : $"--message \"{request.MessageFormat}\"";
        var optOutput = $"--output \"{request.Preset.Name}.txt\"";

        var optName = $"--name \"{request.Preset.Name}\"";
        var optStartDate = $"--start-date {request.Preset.StartDate:yyyy-MM-dd}";
        var optData = $"--data {request.Preset.CanvasDataString}";

        var cmdElements = new string[] { cmdBase, optMessageFormat, optOutput, "new", optName, optStartDate, optData };
        var output = string.Join(' ', cmdElements.Where(x => x != string.Empty));

        return ValueTask.FromResult<Result<string>>(output);
    }
}

