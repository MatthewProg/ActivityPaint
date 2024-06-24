using ActivityPaint.Core.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ActivityPaint.Client.Components.Editor;

public partial class EditorModel : ObservableObject
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private bool? _isDarkModeDefault;

    [ObservableProperty]
    private DateTime? _startDate;

    [ObservableProperty]
    private List<IntensityEnum>? _canvasData;
}
