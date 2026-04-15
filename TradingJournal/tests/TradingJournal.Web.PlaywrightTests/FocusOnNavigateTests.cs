using Microsoft.Playwright;

namespace TradingJournal.Web.PlaywrightTests;

public sealed class FocusOnNavigateTests : IClassFixture<PlaywrightTestHostFixture>
{
    private readonly PlaywrightTestHostFixture _fixture;

    public FocusOnNavigateTests(PlaywrightTestHostFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("/", "Trading Journal")]
    [InlineData("/not-found", "Not Found")]
    [InlineData("/does-not-exist", "Not Found")]
    public async Task FocusOnNavigate_Route_FocusesExpectedPrimaryHeading(string route, string headingText)
    {
        await AssertPrimaryHeadingFocusedAsync(route, headingText);
    }

    private async Task AssertPrimaryHeadingFocusedAsync(string route, string headingText)
    {
        await using var context = await _fixture.Browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync(_fixture.BaseUrl + route);

        var heading = page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions
        {
            Level = 1,
            Name = headingText
        });

        await Assertions.Expect(heading).ToBeVisibleAsync();
        await Assertions.Expect(heading).ToBeFocusedAsync();
    }
}

