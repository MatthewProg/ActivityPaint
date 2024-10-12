namespace ActivityPaint.Client.Components.Documentation.Shared.CommandsTable;

public record DocumentationCommandsTableModel(
    string Name,
    string Description,
    string? DocsUrl = null
);