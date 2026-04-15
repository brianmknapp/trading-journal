using System.Diagnostics;
using Microsoft.Playwright;

namespace TradingJournal.Web.PlaywrightTests;

public sealed class PlaywrightTestHostFixture : IAsyncLifetime
{
    private const string AppUrl = "http://127.0.0.1:5188";
    private Process? _appProcess;

    public IPlaywright Playwright { get; private set; } = null!;

    public IBrowser Browser { get; private set; } = null!;

    public string BaseUrl => AppUrl;

    public async Task InitializeAsync()
    {
        StartAppProcess();
        await WaitForAppAsync();

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    public async Task DisposeAsync()
    {
        if (Browser is not null)
        {
            await Browser.DisposeAsync();
        }

        Playwright?.Dispose();

        if (_appProcess is { HasExited: false })
        {
            try
            {
                _appProcess.Kill(entireProcessTree: true);
            }
            catch
            {
                // Ignore cleanup failures in test teardown.
            }
        }

        _appProcess?.Dispose();
    }

    private void StartAppProcess()
    {
        var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var webProjectPath = Path.Combine(repoRoot, "src", "TradingJournal.Web", "TradingJournal.Web.csproj");

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{webProjectPath}\" --urls {AppUrl}",
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _appProcess = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start TradingJournal.Web test host process.");
    }

    private async Task WaitForAppAsync()
    {
        using var httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(2)
        };

        var start = DateTime.UtcNow;
        var timeout = TimeSpan.FromSeconds(45);

        while (DateTime.UtcNow - start < timeout)
        {
            if (_appProcess is { HasExited: true })
            {
                var stdErr = await _appProcess.StandardError.ReadToEndAsync();
                throw new InvalidOperationException($"Web host exited before startup. {stdErr}");
            }

            try
            {
                using var response = await httpClient.GetAsync(AppUrl);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
                // Retry until timeout while the dev server warms up.
            }

            await Task.Delay(250);
        }

        throw new TimeoutException($"Timed out waiting for web host at {AppUrl}.");
    }
}


