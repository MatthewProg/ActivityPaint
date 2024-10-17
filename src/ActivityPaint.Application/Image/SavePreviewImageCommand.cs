using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Image;

public record SavePreviewImageCommand(
    PresetModel Preset,
    bool? DarkModeOverwrite = null,
    string? Path = null,
    bool Overwrite = false
) : IResultRequest;

internal class SavePreviewImageCommandValidator : AbstractValidator<SavePreviewImageCommand>
{
    public SavePreviewImageCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);

        RuleFor(x => x.Path)
            .Path();
    }
}

internal class SavePreviewImageCommandHandler(IMediator mediator) : IResultRequestHandler<SavePreviewImageCommand>
{
    private readonly IMediator _mediator = mediator;

    public async ValueTask<Result> Handle(SavePreviewImageCommand request, CancellationToken cancellationToken)
    {
        var generateCommand = new GeneratePreviewImageCommand(request.Preset, request.DarkModeOverwrite);
        var generateResult = await _mediator.Send(generateCommand, cancellationToken);

        if (generateResult.IsFailure)
        {
            return generateResult.Error;
        }

        using var stream = generateResult.Value!;
        var saveCommands = new SaveToFileCommand(stream, $"{request.Preset.Name}.png", request.Path, request.Overwrite);

        return await _mediator.Send(saveCommands, cancellationToken);
    }
}