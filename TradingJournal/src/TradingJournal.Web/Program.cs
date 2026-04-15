using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TradingJournal.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

var apiBaseUrl = builder.Configuration["Api:BaseUrl"];
var apiBaseUri = ResolveApiBaseUri(apiBaseUrl, builder.HostEnvironment.BaseAddress);
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = apiBaseUri });

await builder.Build().RunAsync();

static Uri ResolveApiBaseUri(string? configuredApiBaseUrl, string hostBaseAddress)
{
    if (!string.IsNullOrWhiteSpace(configuredApiBaseUrl)
        && Uri.TryCreate(configuredApiBaseUrl, UriKind.Absolute, out var configuredUri))
    {
        return configuredUri;
    }

    return new Uri(hostBaseAddress, UriKind.Absolute);
}

