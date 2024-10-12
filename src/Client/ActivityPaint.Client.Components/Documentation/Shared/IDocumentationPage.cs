using ActivityPaint.Client.Components.Documentation.Shared.CommandsTable;
using ActivityPaint.Client.Components.Documentation.Shared.OptionsTable;
using ActivityPaint.Client.Components.Documentation.Shared.Overview;

namespace ActivityPaint.Client.Components.Documentation.Shared;

public interface IDocumentationPage
{
    public string Title { get; }
    public DocumentationOverviewCommandModel Command { get; }
    public DocumentationCommandsTableModel Parent { get; }
    public IEnumerable<string> Aliases { get; }
    public IEnumerable<DocumentationOptionsTableModel> Options { get; }
    public IEnumerable<DocumentationOptionsTableModel> Arguments { get; }
    public IEnumerable<DocumentationCommandsTableModel> Commands { get; }
    public IEnumerable<string> Examples { get; }
}
