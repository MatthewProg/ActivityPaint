using ActivityPaint.Client.Console.Commands.Save;
using ActivityPaint.Client.Console.Config;
using Spectre.Console.Cli;

var app = new CommandApp(DependencyInjectionFactory.GetTypeRegistrar());

app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif

    config.AddCommand<SaveCommand>("save")
          .WithAlias("s")
          .WithDescription("Saves canvas data as a preset file in the specified location.");
});

return await app.RunAsync(args);
