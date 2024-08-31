using ActivityPaint.Core.Shared.Result;
using System.Globalization;

namespace ActivityPaint.Application.DTOs.Shared.Validators;

public static class CommonValidators
{
    private static readonly char[] InvalidPathChars = Path.GetInvalidFileNameChars()
                                                          .Where(x => x is not '\\' and not '/' and not ':')
                                                          .ToArray();

    public static bool ValidateRequired<T>(T? value, out Result result)
    {
        if (value is null
            || (value is string valueStr && string.IsNullOrWhiteSpace(valueStr)))
        {
            result = ValidationError("Value is required.");
            return false;
        }

        result = Result.Success();
        return true;
    }

    public static bool ValidateDateString(string? value, string format, out Result result)
    {
        if (!DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            result = ValidationError($"Value is not a valid date in '{format}' format.");
            return false;
        }

        result = Result.Success();
        return true;
    }

    public static bool ValidatePath(string? value, out Result result)
    {
        if (!ValidateRequired(value, out result))
        {
            return false;
        }

        if (value!.IndexOfAny(InvalidPathChars) != -1)
        {
            result = ValidationError("Value contains invalid characters.");
            return false;
        }

        if (!Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var parsedUri))
        {
            result = ValidationError("Value is not a valid path.");
            return false;
        }

        if (parsedUri.IsAbsoluteUri
            && (parsedUri.Scheme is "http" or "https"
                || parsedUri.HostNameType is UriHostNameType.IPv4 or UriHostNameType.IPv6))
        {
            result = ValidationError("Value cannot be a network path.");
            return false;
        }

        result = Result.Success();
        return true;
    }

    private static Result ValidationError(string message)
        => new Error("Error.Validation", message);
}
