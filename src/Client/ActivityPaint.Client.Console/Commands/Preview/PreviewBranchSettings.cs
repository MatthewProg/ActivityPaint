using ActivityPaint.Application.BusinessLogic.Image.Models;
using ActivityPaint.Client.Console.Commands.Shared;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Preview;

public class PreviewBranchSettings : CommandSettings
{
    [CommandOption("--mode-overwrite")]
    [Description("If specified, the given mode will be used instead of the one set in the preset. Possible values: light, dark.")]
    [TypeConverter(typeof(StringToEnumConverter<ModeEnum>))]
    public ModeEnum? ModeOverwrite { get; set; }
}
