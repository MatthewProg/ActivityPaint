using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Client.Console.Validators;
using Mediator;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Preview;

public class PreviewSaveCommandSettings : PreviewBranchSettings
{
    [CommandArgument(0, "<PRESET>")]
    [Description("Input preset file path.")]
    public required string InputPath { get; set; }

    [CommandArgument(1, "<OUTPUT>")]
    [Description("Path to the output PNG image file.")]
    public required string OutputPath { get; set; }

    [CommandOption("--force")]
    [Description("Overwrite the existing file.")]
    public bool Overwrite { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.InputPath)
                   .ValidatePath(this, x => x.InputPath)
                   .ValidateRequired(this, x => x.OutputPath)
                   .ValidatePath(this, x => x.OutputPath);
    }
}

public class PreviewSaveCommand(IErrorFeedbackService errorFeedback, IMediator mediator) : AsyncCommand<PreviewSaveCommandSettings>
{
    private readonly IErrorFeedbackService _errorFeedback = errorFeedback;
    private readonly IMediator _mediator = mediator;

    public override async Task<int> ExecuteAsync(CommandContext context, PreviewSaveCommandSettings settings)
    {
        var loadCommand = new LoadPresetCommand(settings.InputPath);
        var loadResult = await _mediator.Send(loadCommand);

        if (loadResult.IsFailure)
        {
            _errorFeedback.WriteError(loadResult.Error);
            return -1;
        }

        var preset = loadResult.Value;
        if (preset is null)
        {
            _errorFeedback.WriteError("Unable to load the preset.");
            return -1;
        }

        var saveCommand = settings.ToSaveCommand(preset);
        var saveResult = await _mediator.Send(saveCommand);

        if (saveResult.IsSuccess)
        {
            return 0;
        }

        _errorFeedback.WriteError(saveResult.Error);

        return -1;
    }
}
