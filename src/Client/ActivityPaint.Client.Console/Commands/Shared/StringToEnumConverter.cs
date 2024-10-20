using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ActivityPaint.Client.Console.Commands.Shared;

public class StringToEnumConverter<T> : TypeConverter where T : struct, Enum
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string);

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        => destinationType == typeof(T);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is null)
            throw GetConvertFromException(value);

        if (value is not string strValue)
            throw GetConvertFromException(value);

        if (!Enum.TryParse<T>(strValue, true, out var enumResult))
            throw GetConvertFromException(value);

        return enumResult;
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is null)
            throw GetConvertToException(value, destinationType);

        if (value is not T enumValue)
            throw GetConvertToException(value, destinationType);

        return Enum.GetName(enumValue) ?? throw GetConvertToException(value, destinationType);
    }
}
