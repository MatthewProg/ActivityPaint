using ActivityPaint.Client.Console.Commands.Generate;
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

    config.AddBranch("generate", generate =>
    {
        generate.AddCommand<GenerateNewCommand>("new")
                .WithAlias("n")
                .WithDescription("Generate repository by providing all the details as arguments.");

        generate.AddCommand<GenerateLoadCommand>("load")
                .WithAlias("l")
                .WithDescription("Generate repository using a preset file.");
    }).WithAlias("g");
});

return await app.RunAsync(args);
