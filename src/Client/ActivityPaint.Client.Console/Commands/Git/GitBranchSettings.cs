using ActivityPaint.Client.Console.Commands.Shared;
using ActivityPaint.Client.Console.Validators;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Git;

public abstract class GitBranchSettings : CommandSettings
{
    [CommandOption("--force")]
    [Description("Overwrite the existing file (relevant only when --output is used).")]
    public bool Overwrite { get; set; }

    [CommandOption("-o|--output")]
    [Description("Path to the output txt file with all the commands.")]
    public required string OutputPath { get; set; }

    [CommandOption("-m|--message")]
    [Description("Commit message format.")]
    [ConfigurationDefaultValue<string>("Repo:MessageFormat")]
    public required string MessageFormat { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.MessageFormat);
    }
}
