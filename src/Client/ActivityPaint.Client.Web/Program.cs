using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Client.Components;
using ActivityPaint.Client.Web;
using ActivityPaint.Client.Web.Interactions;
using ActivityPaint.Integration.FileSystem;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddClientComponents();
builder.Services.AddFileSystemIntegration();

builder.Services.AddScoped<IFileSystemInteraction, FileSystemInteraction>();

builder.Services.ValidateComponentsDI();

await builder.Build().RunAsync();
