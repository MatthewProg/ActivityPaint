using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Shared.Extensions;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Gallery;

public record SaveGalleryItemCommand(
    PresetModel Preset
) : IResultRequest;

internal class SaveGalleryItemCommandValidator : AbstractValidator<SaveGalleryItemCommand>
{
    public SaveGalleryItemCommandValidator(IEnumerable<IValidator<PresetModel>> presetValidators)
    {
        RuleFor(x => x.Preset)
            .NotNull()
            .SetDefaultValidator(presetValidators);
    }
}

internal class SaveGalleryItemCommandHandler(IPresetRepository presetRepository, TimeProvider timeProvider) : IResultRequestHandler<SaveGalleryItemCommand>
{
    private readonly IPresetRepository _presetRepository = presetRepository;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async ValueTask<Result> Handle(SaveGalleryItemCommand request, CancellationToken cancellationToken)
    {
        var preset = request.Preset.ToPreset();
        preset.LastUpdated = _timeProvider.GetUtcNow();

        await _presetRepository.InsertAsync(preset, cancellationToken);

        return Result.Success();
    }
}
