using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Client.Console.Commands.Shared;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Client.Console.Validators;
using Mediator;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Generate;

public class GenerateNewCommandSettings : ManualDataSettings
{
    [CommandOption("--zip")]
    [Description("Save generated repository as a zip file instead of .git directory.")]
    public bool ZipMode { get; set; }

    [CommandOption("--force")]
    [Description("Overwrite the existing file (relevant only when --zip is used).")]
    public bool Overwrite { get; set; }

    [CommandOption("-o|--output")]
    [Description("Path to the output zip file or .git directory.")]
    public string? Path { get; set; }

    [CommandOption("--author-name")]
    [Description("Commits author name.")]
    [ConfigurationDefaultValue<string>("Repo:AuthorFullName")]
    public string? AuthorFullName { get; set; }

    [CommandOption("--author-email")]
    [Description("Commits author email.")]
    [ConfigurationDefaultValue<string>("Repo:AuthorEmail")]
    public string? AuthorEmail { get; set; }

    [CommandOption("-m|--message")]
    [Description("Commit message format.")]
    [ConfigurationDefaultValue<string>("Repo:MessageFormat")]
    public string? MessageFormat { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.Path)
                   .ValidatePath(this, x => x.Path);
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

        var savePath = settings.Path ?? "repo.zip";
        var saveResult = await _fileSaveService.SaveFileAsync(savePath, downloadResult.Value!, settings.Overwrite);

        if (saveResult.IsFailure)
        {
            _errorFeedback.WriteError(downloadResult.Error);
            return -1;
        }

        return 0;
    }
}
