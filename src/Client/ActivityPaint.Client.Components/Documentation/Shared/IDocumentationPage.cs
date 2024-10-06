namespace ActivityPaint.Client.Components.Documentation.Shared;

public interface IDocumentationPage
{
    public string Title { get; }
    public string Command { get; }
    public IEnumerable<string> Aliases { get; }
    public IEnumerable<DocumentationOptionsTableModel> Options { get; }
    public IEnumerable<string> Examples { get; }
}
