using Spectre.Console.Cli;

namespace ActivityPaint.Client.Console.Commands.Generate;

public class GenerateLoadCommandSettings : CommandSettings
{
}


public class GenerateLoadCommand : AsyncCommand<GenerateLoadCommandSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, GenerateLoadCommandSettings settings)
    {
        throw new NotImplementedException();
    }
}