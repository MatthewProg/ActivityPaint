using ActivityPaint.Core.Entities;
using FluentValidation;

namespace ActivityPaint.Core.Validators;

public class PresetValidator : AbstractValidator<Preset>
{
    public PresetValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.CanvasData)
            .NotNull();

        RuleForEach(x => x.CanvasData)
            .IsInEnum();
    }
}
