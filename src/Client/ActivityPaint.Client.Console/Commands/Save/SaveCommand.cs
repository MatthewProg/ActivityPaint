﻿using ActivityPaint.Client.Console.Commands.Shared;
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

    [CommandOption("-n|--name")]
    [Description("Set preset default name.")]
    [CurrentYearDefaultValue()]
    public string? Name { get; set; }

    [CommandOption("-o|--output")]
    [Description("Path to the output JSON preset file.")]
    public string? Path { get; set; }

    public override ValidationResult Validate()
    {
        return base.Validate()
                   .ValidateRequired(this, x => x.Name)
                   .ValidateRequired(this, x => x.Path)
                   .ValidatePath(this, x => x.Path);
    }
}

public class SaveCommand : Command<SaveCommandSettings>
{
    private readonly IMediator _mediator;

    public SaveCommand(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override int Execute(CommandContext context, SaveCommandSettings settings)
    {
        throw new NotImplementedException();
    }
}
