namespace ActivityPaint.Client.Components.Documentation.Shared;

public class DocumentationOptionsTableModel
{
    public string Name { get; }
    public string Type { get; }
    public string Default { get; }
    public string Description { get; }

    private DocumentationOptionsTableModel(string name, string type, string @default, string description)
    {
        Name = name;
        Type = type;
        Default = @default;
        Description = description;
    }

    public static DocumentationOptionsTableModel Build<T>(string name, string description, object? defaultValue = null, string? format = null)
    {
        var typeString = defaultValue switch
        {
            DateOnly => "date",
            _ => typeof(T).Name.ToLower(),
        };

        var defaultString = defaultValue switch
        {
            bool x when format is null => x.ToString().ToLower(),
            _ when format is not null => string.Format(format, defaultValue),
            _ => defaultValue?.ToString() ?? string.Empty
        };

        return new(name, typeString, defaultString, description);
    }
}
