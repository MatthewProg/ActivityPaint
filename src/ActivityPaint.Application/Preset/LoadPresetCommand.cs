using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Preset;

public sealed record LoadPresetCommand(
    string? Path = null
) : IResultRequest<PresetModel?>;

internal class LoadPresetCommandValidator : AbstractValidator<LoadPresetCommand>
{
    public LoadPresetCommandValidator()
    {
        RuleFor(x => x.Path)
            .Path();
    }
}

internal class LoadPresetCommandHandler(IMediator mediator) : IResultRequestHandler<LoadPresetCommand, PresetModel?>
{
    private readonly IMediator _mediator = mediator;

    public async ValueTask<Result<PresetModel?>> Handle(LoadPresetCommand command, CancellationToken cancellationToken)
    {
        var loadCommand = new LoadFromFileCommand(command.Path);
        var streamResult = await _mediator.Send(loadCommand, cancellationToken);

        if (streamResult.IsFailure)
        {
            return streamResult.Error;
        }

        using var stream = streamResult.Value!;
        var parseCommand = new ParsePresetCommand(stream);

        return await _mediator.Send(parseCommand, cancellationToken);
    }
}
