using CommunityToolkit.Mvvm.ComponentModel;

namespace ActivityPaint.Client.Components.Configuration;

public partial class ConfigurationModel : ObservableObject
{
    [ObservableProperty]
    private string? _authorFullName;

    [ObservableProperty]
    private string? _authorEmail;

    [ObservableProperty]
    private string? _messageFormat;
}

