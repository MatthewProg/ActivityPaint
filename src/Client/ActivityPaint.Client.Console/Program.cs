using ActivityPaint.Client.Console.Config;
using Spectre.Console.Cli;

var app = new CommandApp(DependencyInjectionFactory.GetTypeRegistrar());

app.Configure(c =>
{
});

return await app.RunAsync(args);
