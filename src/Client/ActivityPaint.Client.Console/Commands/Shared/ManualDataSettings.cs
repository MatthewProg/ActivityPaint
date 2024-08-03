using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Globalization;

namespace ActivityPaint.Client.Console.Commands.Shared;

public class CurrentYearDefaultValueAttribute : DefaultValueAttribute
{
    public CurrentYearDefaultValueAttribute() : base($"{DateTime.UtcNow.Year}-01-01") { }
}

public abstract class ManualDataSettings : CommandSettings
{
    [CommandOption("-d|--data")]
    [Description("Canvas data in an encoded string form.")]
    public string? CanvasData { get; set; }

    [CommandOption("-s|--start-date")]
    [Description("Start date of the canvas data in yyyy-MM-dd format.")]
    [CurrentYearDefaultValue()]
    public string? StartDateString { get; set; }

    public DateTime StartDate
        => DateTime.ParseExact(StartDateString ?? string.Empty, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    public override ValidationResult Validate()
    {
        if (!OptionsValidator.ValidateRequired(CanvasData, "--data", out var result)
            || !OptionsValidator.ValidateRequired(StartDateString, "--start-date", out result)
            || !OptionsValidator.ValidateDateString(StartDateString, "yyyy-MM-dd", "--start-date", out result))
        {
            return result;
        }

        return base.Validate();
    }
}
