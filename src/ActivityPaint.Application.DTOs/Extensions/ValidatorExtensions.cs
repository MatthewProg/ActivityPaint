using FluentValidation;

namespace ActivityPaint.Application.DTOs.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> SetDefaultValidator<T, TProperty>(this IRuleBuilderOptions<T, TProperty> builder, IEnumerable<IValidator<TProperty>> validators)
    {
        var validator = validators.FirstOrDefault();

        if (validator is not null)
        {
            return builder.SetValidator(validator);
        }

        return builder;
    }
}
