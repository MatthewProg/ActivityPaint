using ActivityPaint.Client.Components.Enums;
using ActivityPaint.Core.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ActivityPaint.Client.Components.Models;

public partial class PaintCanvasModel : ObservableObject
{
    [ObservableProperty]
    private bool _isDarkMode;

    [ObservableProperty]
    private int _brushSize;

    [ObservableProperty]
    private PaintToolEnum _selectedTool;

    [ObservableProperty]
    private IntensityEnum _selectedIntensity;
}
