using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Client.Components;
using ActivityPaint.Client.Web.Interactions;
using ActivityPaint.Integration.Database;
using ActivityPaint.Integration.FileSystem;
using ActivityPaint.Integration.Repository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ActivityPaint.Client.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddClientComponents();
        builder.Services.AddDatabaseIntegration();
        builder.Services.AddFileSystemIntegration();
        builder.Services.AddRepositoryIntegration();

        builder.Services.AddScoped<IFileSystemInteraction, FileSystemInteraction>();

        builder.Services.ValidateComponentsDI();

        await builder.Build().RunAsync();
    }
}