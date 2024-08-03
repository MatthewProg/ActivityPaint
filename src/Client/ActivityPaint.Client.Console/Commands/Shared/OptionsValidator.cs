using Spectre.Console;
using System.Globalization;

namespace ActivityPaint.Client.Console.Commands.Shared;

public static class OptionsValidator
{
    private static readonly char[] InvalidPathChars = Path.GetInvalidFileNameChars()
                                                          .Where(x => x is not '\\' and not '/' and not ':')
                                                          .ToArray();
    public static bool ValidateRequired(string? value, string optionName, out ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = ValidationResult.Error($"'{optionName}' option is required.");
            return false;
        }

        result = ValidationResult.Success();
        return true;
    }

    public static bool ValidateRequired<T>(T? value, string optionName, out ValidationResult result) where T : class
    {
        if (value is null)
        {
            result = ValidationResult.Error($"'{optionName}' option is required.");
            return false;
        }

        result = ValidationResult.Success();
        return true;
    }

    public static bool ValidateDateString(string? value, string format, string optionName, out ValidationResult result)
    {
        if (!DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            result = ValidationResult.Error($"'{optionName}' value is not a valid date in '{format}' format.");
            return false;
        }

        result = ValidationResult.Success();
        return true;
    }

    public static bool ValidatePath(string? value, string optionName, out ValidationResult result)
    {
        if (!ValidateRequired(value, optionName, out result))
        {
            return false;
        }

        if (value!.IndexOfAny(InvalidPathChars) != -1)
        {
            result = ValidationResult.Error($"'{optionName}' value contains invalid characters.");
            return false;
        }

        if (!Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var parsedUri))
        {
            result = ValidationResult.Error($"'{optionName}' value is not a valid path.");
            return false;
        }

        if (parsedUri.IsAbsoluteUri && !parsedUri.IsFile)
        {
            result = ValidationResult.Error($"'{optionName}' value is not a valid file path.");
            return false;
        }

        result = ValidationResult.Success();
        return true;
    }
}
