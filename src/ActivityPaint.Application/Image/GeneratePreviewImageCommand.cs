using ActivityPaint.Application.BusinessLogic.Image.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Image;

public record GeneratePreviewImageCommand(
    PresetModel Preset,
    bool? DarkModeOverwrite = null
) : IResultRequest<byte[]>;

internal class GeneratePreviewImageCommandValidator : AbstractValidator<GeneratePreviewImageCommand>
{
    public GeneratePreviewImageCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class GeneratePreviewImageCommandHandler(IPreviewImageService previewImageService) : IResultRequestHandler<GeneratePreviewImageCommand, byte[]>
{
    private readonly IPreviewImageService _previewImageService = previewImageService;

    public async ValueTask<Result<byte[]>> Handle(GeneratePreviewImageCommand request, CancellationToken cancellationToken)
    {
        return await _previewImageService.GeneratePreviewAsync(request.Preset, request.DarkModeOverwrite, cancellationToken);
    }
}