using ActivityPaint.Client.Console.Commands.Generate;
using ActivityPaint.Client.Console.Commands.Git;
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

    config.AddBranch<GenerateBranchSettings>("generate", generate =>
    {
        generate.SetDescription("Generate repository based on a preset file or arguments provided.");

        generate.AddCommand<GenerateNewCommand>("new")
                .WithAlias("n")
                .WithDescription("Generate repository by providing all the details as arguments.");

        generate.AddCommand<GenerateLoadCommand>("load")
                .WithAlias("l")
                .WithDescription("Generate repository using a preset file.");
    }).WithAlias("g");

    config.AddBranch<GitBranchSettings>("git", git =>
    {
        git.SetDescription("Generate git commands to create the repository based on a preset file or arguments provided.");

        git.AddCommand<GitNewCommand>("new")
           .WithAlias("n")
           .WithDescription("Generate git commands by providing all the details as arguments.");

        git.AddCommand<GitLoadCommand>("load")
           .WithAlias("l")
           .WithDescription("Generate git commands using a preset file.");
    });
});

return await app.RunAsync(args);
