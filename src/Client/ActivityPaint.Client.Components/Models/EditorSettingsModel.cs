using ActivityPaint.Client.Components.Enums;
using ActivityPaint.Core.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ActivityPaint.Client.Components.Models;

public partial class EditorSettingsModel : ObservableObject
{
    [ObservableProperty]
    private bool _isDarkMode;

    [ObservableProperty]
    private DateTime? _startDate;

    [ObservableProperty]
    private int _brushSize;

    [ObservableProperty]
    private EditorToolEnum _selectedTool;

    [ObservableProperty]
    private IntensityEnum _selectedIntensity;
}
