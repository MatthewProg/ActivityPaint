using ActivityPaint.Client.Console.Commands.Shared;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Client.Console.Validators;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using Mediator;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Globalization;

namespace ActivityPaint.Client.Console.Commands.Generate;

public class GenerateNewCommandSettings : GenerateBranchSettings
{
    [CommandOption("-n|--name")]
    [Description("Set preset default name.")]
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

public class GenerateNewCommand(IErrorFeedbackService errorFeedback, IMediator mediator) : AsyncCommand<GenerateNewCommandSettings>
{
    private readonly IErrorFeedbackService _errorFeedback = errorFeedback;
    private readonly IMediator _mediator = mediator;

    public override async Task<int> ExecuteAsync(CommandContext context, GenerateNewCommandSettings settings)
    {
        return await AnsiConsole.Progress()
                                .StartAsync(async ctx =>
                                {
                                    var progressTask = ctx.AddTask("Repository generation");

                                    return await Generate(progressTask, settings);
                                });
    }

    private async Task<int> Generate(ProgressTask progressTask, GenerateNewCommandSettings settings)
    {
        var generateCommand = settings.ToGenerateRepoCommand(x => progressTask.MaxValue(x.Count).Value = x.Current);
        progressTask.StartTask();

        var generateResult = await _mediator.Send(generateCommand);

        if (generateResult.IsFailure)
        {
            _errorFeedback.WriteError(generateResult.Error);
            return -1;
        }

        return 0;
    }
}
