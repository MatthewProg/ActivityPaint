using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Preview;

public class PreviewBranchSettings : CommandSettings
{
    [CommandOption("--dark-mode-overwrite")]
    [Description("If specified, the light or dark mode will be used instead of the one set in the preset.")]
    public bool? DarkModeOverwrite { get; set; }
}
