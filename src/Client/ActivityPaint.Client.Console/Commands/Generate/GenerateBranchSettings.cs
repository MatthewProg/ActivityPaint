using ActivityPaint.Client.Console.Commands.Shared;
using ActivityPaint.Client.Console.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Generate;

public abstract class GenerateBranchSettings : CommandSettings
{
    [CommandOption("--zip")]
    [Description("Save generated repository as a zip file instead of .git directory.")]
    public bool ZipMode { get; set; }

    [CommandOption("--force")]
    [Description("Overwrite the existing file (relevant only when --zip is used).")]
    public bool Overwrite { get; set; }

    [CommandOption("-o|--output")]
    [Description("Path to the output zip file or .git directory.")]
    public required string OutputPath { get; set; }

    [CommandOption("--author-name")]
    [Description("Commits author name.")]
    [ConfigurationDefaultValue<string>("Repo:AuthorFullName")]
    public required string AuthorFullName { get; set; }

    [CommandOption("--author-email")]
    [Description("Commits author email.")]
    [ConfigurationDefaultValue<string>("Repo:AuthorEmail")]
    public required string AuthorEmail { get; set; }

    [CommandOption("-m|--message")]
    [Description("Commit message format.")]
    [ConfigurationDefaultValue<string>("Repo:MessageFormat")]
    public required string MessageFormat { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.AuthorFullName)
                   .ValidateRequired(this, x => x.AuthorEmail)
                   .ValidateRequired(this, x => x.MessageFormat)
                   .ValidateRequired(this, x => x.OutputPath)
                   .ValidatePath(this, x => x.OutputPath);
    }
}
