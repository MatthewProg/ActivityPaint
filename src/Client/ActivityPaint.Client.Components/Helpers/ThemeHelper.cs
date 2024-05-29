using ActivityPaint.Core.Enums;
using MudBlazor;
using Color = System.Drawing.Color;

namespace ActivityPaint.Client.Components.Helpers;

public static class ThemeHelper
{
    public static MudTheme GetDefaultTheme()
    {
        var theme = new MudTheme();

        theme.PaletteLight.Primary = theme.PaletteLight.Dark;
        theme.PaletteLight.PrimaryDarken = theme.PaletteLight.DarkDarken;
        theme.PaletteLight.PrimaryLighten = theme.PaletteLight.DarkLighten;
        theme.PaletteDark.Primary = theme.PaletteDark.GrayDarker;
        theme.PaletteDark.PrimaryDarken = theme.PaletteDark.GrayDark;
        theme.PaletteDark.PrimaryLighten = theme.PaletteDark.GrayLight;

        return theme;
    }

    public static Color GetIntensityColor(IntensityEnum intensity, bool isDarkMode) => intensity switch
    {
        IntensityEnum.Level0 when !isDarkMode => Color.FromArgb(255, 235, 237, 240),
        IntensityEnum.Level1 when !isDarkMode => Color.FromArgb(255, 155, 233, 168),
        IntensityEnum.Level2 when !isDarkMode => Color.FromArgb(255, 64, 196, 99),
        IntensityEnum.Level3 when !isDarkMode => Color.FromArgb(255, 48, 161, 78),
        IntensityEnum.Level4 when !isDarkMode => Color.FromArgb(255, 33, 110, 57),
        IntensityEnum.Level0 when isDarkMode => Color.FromArgb(255, 22, 27, 34),
        IntensityEnum.Level1 when isDarkMode => Color.FromArgb(255, 14, 68, 41),
        IntensityEnum.Level2 when isDarkMode => Color.FromArgb(255, 0, 109, 50),
        IntensityEnum.Level3 when isDarkMode => Color.FromArgb(255, 38, 166, 65),
        IntensityEnum.Level4 when isDarkMode => Color.FromArgb(255, 57, 211, 83),
        _ => throw new NotImplementedException("Unsupported intensity and theme combination")
    };
}
