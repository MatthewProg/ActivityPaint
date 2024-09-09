using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Cli;

public sealed record CliCmdGenerateRepoCommand(
    PresetModel Preset,
    AuthorModel Author,
    string? MessageFormat = null
) : IResultRequest<string>;

internal class CliCmdGenerateRepoCommandValidator : AbstractValidator<CliCmdGenerateRepoCommand>
{
    public CliCmdGenerateRepoCommandValidator(
        IEnumerable<IValidator<PresetModel>> presetValidators,
        IEnumerable<IValidator<AuthorModel>> authorValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);

        RuleFor(x => x.Author)
            .NotNull()
            .SetDefaultValidator(authorValidators);
    }
}

internal class CliCmdGenerateRepoCommandHandler : IResultRequestHandler<CliCmdGenerateRepoCommand, string>
{
    public ValueTask<Result<string>> Handle(CliCmdGenerateRepoCommand request, CancellationToken cancellationToken)
    {
        var cmdBase = "ap-cli.exe generate";

        var optMessageFormat = request.MessageFormat is null ? string.Empty : $"--message \"{request.MessageFormat}\"";
        var optAuthorName = $"--author-name \"{request.Author.FullName}\"";
        var optAuthorEmail = $"--author-email \"{request.Author.Email}\"";
        var optZipMode = "--zip";
        var optOutput = $"--output \"{request.Preset.Name}.zip\"";

        var optName = $"--name \"{request.Preset.Name}\"";
        var optStartDate = $"--start-date {request.Preset.StartDate:yyyy-MM-dd}";
        var optData = $"--data {request.Preset.CanvasDataString}";

        var cmdElements = new string[] { cmdBase, optMessageFormat, optAuthorName, optAuthorEmail, optZipMode, optOutput, "new", optName, optStartDate, optData };
        var output = string.Join(' ', cmdElements.Where(x => x != string.Empty));

        return ValueTask.FromResult<Result<string>>(output);
    }
}
