using ActivityPaint.Core.Shared.Result;
using MudBlazor;

namespace ActivityPaint.Client.Components.Shared.Services;

public interface IFeedbackService
{
    void Show(string message, Severity severity);
    void ShowError(Error error);
    void ClearAll();
}

public class FeedbackService : IFeedbackService
{
    private readonly ISnackbar _snackbar;

    public FeedbackService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void Show(string message, Severity severity)
    {
        _snackbar.Add(message, severity);
    }

    public void ShowError(Error error)
    {
        _snackbar.Add(error.Message, Severity.Error);
    }

    public void ClearAll()
    {
        _snackbar.Clear();
    }
}
