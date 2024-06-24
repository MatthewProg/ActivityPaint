using MudBlazor;
using System.Diagnostics.CodeAnalysis;

namespace ActivityPaint.Client.Components.Shared.Theming;

public class ThemeModel
{
    public required MudTheme MudTheme { get; set; }
    public required ThemeEnum Theme { get; set; }
    public ThemeEnum? ResolvedTheme { get; private set; }

    public event EventHandler<ThemeEnum>? OnThemeResolved;

    public bool IsDarkMode => (ResolvedTheme ?? Theme) == ThemeEnum.Dark;

    [SetsRequiredMembers]
    public ThemeModel()
    {
        MudTheme = ThemeHelper.GetDefaultTheme();
        Theme = ThemeEnum.System;
        ResolvedTheme = null;
    }

    public void ResolveTheme(ThemeEnum theme)
    {
        ResolvedTheme = theme;
        OnThemeResolved?.Invoke(this, theme);
    }
}
