using ActivityPaint.Integration.Database;
using ActivityPaint.Integration.Database.SeedData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.AddConsole();

builder.Services.AddDatabaseIntegration();

builder.Services.AddSingleton<IDatabaseSeedService, DatabaseSeedService>();
builder.Services.AddHostedService<MainHostedService>();

var host = builder.Build();

await host.RunAsync();
