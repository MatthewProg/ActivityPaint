using ActivityPaint.Client.Console.Validators;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Globalization;

namespace ActivityPaint.Client.Console.Commands.Shared;

public abstract class ManualDataSettings : CommandSettings
{
    [CommandOption("-n|--name")]
    [Description("Preset name.")]
    [CurrentYearDefaultValue]
    public required string Name { get; set; }

    [CommandOption("-d|--data")]
    [Description("Canvas data in an encoded string form.")]
    public required string CanvasDataString { get; set; }

    public List<IntensityEnum> CanvasData
        => CanvasDataHelper.ConvertToList(CanvasDataString);

    [CommandOption("-s|--start-date")]
    [Description("Start date of the canvas data in yyyy-MM-dd format.")]
    [CurrentYearDefaultValue]
    public required string StartDateString { get; set; }

    public DateTime StartDate
        => DateTime.ParseExact(StartDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.Name)
                   .ValidateRequired(this, x => x.CanvasDataString)
                   .ValidateRequired(this, x => x.StartDateString)
                   .ValidateDateString(this, "yyyy-MM-dd", x => x.StartDateString);
    }
}
