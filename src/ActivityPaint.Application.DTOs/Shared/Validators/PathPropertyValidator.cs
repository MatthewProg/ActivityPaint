using FluentValidation;
using FluentValidation.Validators;

namespace ActivityPaint.Application.DTOs.Shared.Validators;

public class PathPropertyValidator<T> : PropertyValidator<T, string?>
{
    public override string Name => "PathPropertyValidator";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        return CommonValidators.ValidatePath(value, out _);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "{PropertyName} must be an empty value or a valid path.";
}
