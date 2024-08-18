using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.BusinessLogic.Shared.Preset;
using ActivityPaint.Application.DTOs.Models;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using System.Text.Json;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public record ParsePresetCommand(
    Stream PresetStream
) : IResultRequest<PresetModel?>;

internal class ParsePresetCommandValidator : AbstractValidator<ParsePresetCommand>
{
    public ParsePresetCommandValidator()
    {
        RuleFor(x => x.PresetStream)
            .NotNull();
    }
}

internal class ParsePresetCommandHandler : IResultRequestHandler<ParsePresetCommand, PresetModel?>
{
    public async ValueTask<Result<PresetModel?>> Handle(ParsePresetCommand command, CancellationToken cancellationToken)
    {
        var fileModel = await JsonSerializer.DeserializeAsync<PresetFileModel>(command.PresetStream, cancellationToken: cancellationToken);

        return fileModel?.ToPresetModel();
    }
}