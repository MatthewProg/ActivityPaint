using ActivityPaint.Application.Abstractions.FileSystem;
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
    [CurrentYearDefaultValue()]
    public required string Name { get; set; }

    [CommandOption("-d|--data")]
    [Description("Canvas data in an encoded string form.")]
    public required string CanvasDataString { get; set; }

    public List<IntensityEnum> CanvasData
        => CanvasDataHelper.ConvertToList(CanvasDataString);

    [CommandOption("-s|--start-date")]
    [Description("Start date of the canvas data in yyyy-MM-dd format.")]
    [CurrentYearDefaultValue()]
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

public class GenerateNewCommand : AsyncCommand<GenerateNewCommandSettings>
{
    private readonly IErrorFeedbackService _errorFeedback;
    private readonly IFileSaveService _fileSaveService;
    private readonly IMediator _mediator;

    public GenerateNewCommand(IErrorFeedbackService errorFeedback, IFileSaveService fileSaveService, IMediator mediator)
    {
        _fileSaveService = fileSaveService;
        _errorFeedback = errorFeedback;
        _mediator = mediator;
    }

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
        if (!settings.ZipMode)
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

        var downloadCommand = settings.ToDownloadRepoCommand(x => progressTask.MaxValue(x.Count).Value = x.Current);
        progressTask.StartTask();

        var downloadResult = await _mediator.Send(downloadCommand);

        if (downloadResult.IsFailure)
        {
            _errorFeedback.WriteError(downloadResult.Error);
            return -1;
        }

        var saveResult = await _fileSaveService.SaveFileAsync(settings.OutputPath, downloadResult.Value!, settings.Overwrite);

        if (saveResult.IsFailure)
        {
            _errorFeedback.WriteError(downloadResult.Error);
            return -1;
        }

        return 0;
    }
}
