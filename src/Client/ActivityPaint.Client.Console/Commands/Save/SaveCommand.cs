using ActivityPaint.Client.Console.Commands.Shared;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Client.Console.Validators;
using Mediator;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Save;

public class SaveCommandSettings : ManualDataSettings
{
    [CommandOption("--dark-mode")]
    [Description("Use dark mode as a default preview theme.")]
    public bool IsDarkModeDefault { get; set; }

    [CommandOption("-o|--output")]
    [Description("Path to the output JSON preset file.")]
    public required string Path { get; set; }

    [CommandOption("--force")]
    [Description("Overwrite the existing file.")]
    public bool Overwrite { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.Path)
                   .ValidatePath(this, x => x.Path);
    }
}

public class SaveCommand : AsyncCommand<SaveCommandSettings>
{
    private readonly IErrorFeedbackService _errorFeedback;
    private readonly IMediator _mediator;

    public SaveCommand(IErrorFeedbackService errorFeedback, IMediator mediator)
    {
        _errorFeedback = errorFeedback;
        _mediator = mediator;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SaveCommandSettings settings)
    {
        var command = settings.ToSavePresetCommand();

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return 0;
        }

        _errorFeedback.WriteError(result.Error);

        return -1;
    }
}
