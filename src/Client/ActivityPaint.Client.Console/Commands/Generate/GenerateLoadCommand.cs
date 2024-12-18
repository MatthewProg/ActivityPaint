using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Client.Console.Validators;
using Mediator;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Generate;

public class GenerateLoadCommandSettings : GenerateBranchSettings
{
    [CommandArgument(0, "<FILE>")]
    [Description("Input preset file path.")]
    public required string InputPath { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.InputPath)
                   .ValidatePath(this, x => x.InputPath);
    }
}


public class GenerateLoadCommand(IErrorFeedbackService errorFeedback, IMediator mediator) : AsyncCommand<GenerateLoadCommandSettings>
{
    private readonly IErrorFeedbackService _errorFeedback = errorFeedback;
    private readonly IMediator _mediator = mediator;

    public override async Task<int> ExecuteAsync(CommandContext context, GenerateLoadCommandSettings settings)
    {
        return await AnsiConsole.Progress()
                                .StartAsync(async ctx =>
                                {
                                    var progressTask = ctx.AddTask("Repository generation");

                                    return await Generate(progressTask, settings);
                                });
    }

    private async Task<int> Generate(ProgressTask progressTask, GenerateLoadCommandSettings settings)
    {
        var loadPresetCommand = new LoadPresetCommand(settings.InputPath);
        var loadPresetResult = await _mediator.Send(loadPresetCommand);

        if (loadPresetResult.IsFailure)
        {
            _errorFeedback.WriteError(loadPresetResult.Error);
            return -1;
        }

        var preset = loadPresetResult.Value;
        if (preset is null)
        {
            _errorFeedback.WriteError("Preset is empty!");
            return -1;
        }

        var generateCommand = settings.ToGenerateRepoCommand(preset, x => progressTask.MaxValue(x.Count).Value = x.Current);
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
