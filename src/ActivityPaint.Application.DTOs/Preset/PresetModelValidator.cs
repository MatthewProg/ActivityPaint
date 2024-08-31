using FluentValidation;

namespace ActivityPaint.Application.DTOs.Preset;

public class PresetModelValidator : AbstractValidator<PresetModel>
{
    public PresetModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.CanvasData)
            .NotNull();

        RuleForEach(x => x.CanvasData)
            .IsInEnum();
    }
}
