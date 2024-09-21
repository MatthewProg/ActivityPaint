using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Client.Console.Validators;
using Mediator;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Git;

public class GitLoadCommandSettings : GitBranchSettings
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


public class GitLoadCommand(IErrorFeedbackService errorFeedback, IMediator mediator) : AsyncCommand<GitLoadCommandSettings>
{
    private readonly IErrorFeedbackService _errorFeedback = errorFeedback;
    private readonly IMediator _mediator = mediator;

    public override async Task<int> ExecuteAsync(CommandContext context, GitLoadCommandSettings settings)
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

        var generateCommand = settings.ToGenerateGitCmdCommand(preset);
        var generateResult = await _mediator.Send(generateCommand);
        if (generateResult.IsFailure)
        {
            _errorFeedback.WriteError(generateResult.Error);
            return -1;
        }

        if (string.IsNullOrEmpty(settings.OutputPath))
        {
            AnsiConsole.WriteLine(generateResult.Value!);
            return 0;
        }

        var saveCommand = new SaveTextToFileCommand(generateResult.Value!, settings.OutputPath, settings.Overwrite);
        var saveResult = await _mediator.Send(saveCommand);
        if (saveResult.IsFailure)
        {
            _errorFeedback.WriteError(saveResult.Error);
            return -1;
        }

        return 0;
    }
}
