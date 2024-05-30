using ActivityPaint.Client.Components.Enums;
using ActivityPaint.Client.Components.Helpers;
using MudBlazor;
using System.Diagnostics.CodeAnalysis;

namespace ActivityPaint.Client.Components.Models;

public class ThemeModel
{
    public required MudTheme MudTheme { get; set; }
    public required ThemeEnum Theme { get; set; }
    public ThemeEnum? ResolvedTheme { get; set; }

    public bool IsDarkMode => (ResolvedTheme ?? Theme) == ThemeEnum.Dark;

    [SetsRequiredMembers]
    public ThemeModel()
    {
        MudTheme = ThemeHelper.GetDefaultTheme();
        Theme = ThemeEnum.System;
        ResolvedTheme = null;
    }
}
