using ActivityPaint.Application.DTOs.Shared.Validators;
using FluentValidation;

namespace ActivityPaint.Application.DTOs.Shared.Extensions;

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

    public static IRuleBuilderOptions<T, string?> Path<T>(this IRuleBuilder<T, string?> builder)
        => builder.SetValidator(new PathPropertyValidator<T>());
}
