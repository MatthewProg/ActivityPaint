using ActivityPaint.Application.DTOs.Shared.Validators;
using ActivityPaint.Core.Extensions;
using Spectre.Console;

namespace ActivityPaint.Client.Console.Validators;

public static class OptionsValidator
{
    public static bool ValidateRequired<T>(T? value, string optionName, out ValidationResult result)
    {
        var output = CommonValidators.ValidateRequired<T>(value, out var innerResult);

        result = innerResult.IsSuccess
            ? ValidationResult.Success()
            : ValidationResult.Error(FormatMessage(innerResult.Error.Message, optionName));

        return output;
    }

    public static bool ValidateDateString(string? value, string format, string optionName, out ValidationResult result)
    {
        var output = CommonValidators.ValidateDateString(value, format, out var innerResult);

        result = innerResult.IsSuccess
            ? ValidationResult.Success()
            : ValidationResult.Error(FormatMessage(innerResult.Error.Message, optionName));

        return output;
    }

    public static bool ValidatePath(string? value, string optionName, out ValidationResult result)
    {
        var output = CommonValidators.ValidatePath(value, out var innerResult);

        result = innerResult.IsSuccess
            ? ValidationResult.Success()
            : ValidationResult.Error(FormatMessage(innerResult.Error.Message, optionName));

        return output;
    }

    private static string FormatMessage(string innerErrorMessage, string optionName)
    {
        var normalized = innerErrorMessage switch
        {
            null or "" => "value is invalid",
            { Length: 1 } => innerErrorMessage[0].ToLower().ToString(),
            { Length: > 1 } => $"{innerErrorMessage[0].ToLower()}{innerErrorMessage[1..]}",
            _ => "value is invalid"
        };

        return $"'{optionName}' {normalized}";
    }
}
