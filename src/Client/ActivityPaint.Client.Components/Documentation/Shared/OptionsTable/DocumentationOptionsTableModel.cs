namespace ActivityPaint.Client.Components.Documentation.Shared.OptionsTable;

public record DocumentationOptionsTableModel
{
    public string Name { get; }
    public bool Required { get; }
    public string Type { get; }
    public string Default { get; }
    public string Description { get; }

    private DocumentationOptionsTableModel(string name, bool required, string type, string @default, string description)
    {
        Name = name;
        Required = required;
        Type = type;
        Default = @default;
        Description = description;
    }

    public static DocumentationOptionsTableModel Build<T>(string name, bool required, string description, object? defaultValue = null, string? format = null)
    {
        var typeString = GetTypeName<T>();

        var defaultString = defaultValue switch
        {
            bool x when format is null => x.ToString().ToLower(),
            _ when format is not null => string.Format(format, defaultValue),
            _ => defaultValue?.ToString() ?? string.Empty
        };

        return new(name, required, typeString, defaultString, description);
    }

    private static string GetTypeName<T>()
    {
        var underlyingType = Nullable.GetUnderlyingType(typeof(T));
        var type = underlyingType ?? typeof(T);

        var typeString = typeof(T) switch
        {
            _ when type == typeof(DateOnly) => "date",
            _ when type == typeof(bool) => "bool",
            _ => type.Name.ToLower(),
        };

        return underlyingType is null
            ? typeString
            : $"{typeString}?";
    }
}
