using ActivityPaint.Client.Components.Enums;
using ActivityPaint.Core.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ActivityPaint.Client.Components.Models;

public class EditorSettingsModel : INotifyPropertyChanged
{
    private bool _isDarkMode;
    private DateTime? _startDate;
    private int _brushSize;
    private EditorToolEnum _selectedTool;
    private IntensityEnum _selectedIntensity;

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode == value) return;

            _isDarkMode = value;
            OnPropertyChanged();
        }
    }

    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            if (_startDate == value) return;

            _startDate = value;
            OnPropertyChanged();
        }
    }

    public int BrushSize
    {
        get => _brushSize;
        set
        {
            if (_brushSize == value) return;

            _brushSize = value;
            OnPropertyChanged();
        }
    }

    public EditorToolEnum SelectedTool
    {
        get => _selectedTool;
        set
        {
            if (_selectedTool == value) return;

            _selectedTool = value;
            OnPropertyChanged();
        }
    }

    public IntensityEnum SelectedIntensity
    {
        get => _selectedIntensity;
        set
        {
            if (_selectedIntensity == value) return;

            _selectedIntensity = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
