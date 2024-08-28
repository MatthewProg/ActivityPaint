using ActivityPaint.Core.Extensions;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Linq.Expressions;

namespace ActivityPaint.Client.Console.Validators;

public static class ValidationResultExtensions
{
    public static ValidationResult ValidateRequired<TSource, TProperty>(this ValidationResult result, TSource source, Expression<Func<TSource, TProperty>> expression)
    {
        if (!result.Successful)
        {
            return result;
        }

        var optionName = GetOptionName(expression);
        var value = expression.Compile().Invoke(source);

        _ = OptionsValidator.ValidateRequired(value, optionName, out var validationResult);
        return validationResult;
    }

    public static ValidationResult ValidateDateString<TSource>(this ValidationResult result, TSource source, string format, Expression<Func<TSource, string?>> expression)
    {
        if (!result.Successful)
        {
            return result;
        }

        var optionName = GetOptionName(expression);
        var value = expression.Compile().Invoke(source);

        _ = OptionsValidator.ValidateDateString(value, format, optionName, out var validationResult);
        return validationResult;
    }

    public static ValidationResult ValidatePath<TSource>(this ValidationResult result, TSource source, Expression<Func<TSource, string?>> expression)
    {
        if (!result.Successful)
        {
            return result;
        }

        var optionName = GetOptionName(expression);
        var value = expression.Compile().Invoke(source);

        _ = OptionsValidator.ValidatePath(value, optionName, out var validationResult);
        return validationResult;
    }

    private static string GetOptionName<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
    {
        var propInfo = expression.GetPropertyInfo();

        var argumentAttributes = propInfo.GetCustomAttributes(typeof(CommandArgumentAttribute), false);
        if (argumentAttributes.Length != 0)
        {
            var argAttrib = (CommandArgumentAttribute)argumentAttributes[0];
            return $"<{argAttrib.ValueName}>";
        }

        var optionAttributes = propInfo.GetCustomAttributes(typeof(CommandOptionAttribute), false);
        if (optionAttributes.Length == 0)
        {
            throw new ArgumentException($"Property in '{expression}' expression, is missing CommandOptionAttribute or CommandArgumentAttribute.");
        }

        var attribute = (CommandOptionAttribute)optionAttributes[0];
        var longName = attribute.LongNames.FirstOrDefault();
        if (longName is not null)
        {
            return $"--{longName}";
        }

        var shortName = attribute.ShortNames.FirstOrDefault();
        if (shortName is not null)
        {
            return $"-{shortName}";
        }

        return "unknown";
    }
}
