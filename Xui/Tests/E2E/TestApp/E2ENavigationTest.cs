using System.Runtime.CompilerServices;

namespace Xui.Tests.E2E.TestApp;

/// <summary>
/// End-to-end tests that launch the real TestApp.Desktop process, connect to
/// it via DevTools, and drive navigation through the UI.
///
/// These tests open a real operating-system window (macOS or Windows depending
/// on the host) and exercise the app the same way a human would.
///
/// To run:
///   dotnet test Xui/Tests/E2E/Xui.Tests.E2E.csproj
/// </summary>
public class E2ENavigationTest
{
    // Path to TestApp.Desktop.csproj relative to the solution root.
    private const string TestAppProject = "Xui/Apps/TestApp/TestApp.Desktop.csproj";

    private static string SolutionRoot([CallerFilePath] string callerPath = "")
        => RealApp.FindSolutionRoot(callerPath);

    [Fact]
    public async Task HomePage_Renders_And_Shows_Navigation_Buttons()
    {
        await using var log = new TestLog(nameof(HomePage_Renders_And_Shows_Navigation_Buttons));
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot(),
            testLog: log);

        var root = await app.WaitForWindowAsync();

        // The home page should have navigation buttons for the examples.
        var gridButton = root.FindById("GridLayout");
        Assert.NotNull(gridButton);
        Assert.True(gridButton.Visible);
    }

    [Fact]
    public async Task Can_Navigate_To_GridLayout_And_Back()
    {
        await using var log = new TestLog(nameof(Can_Navigate_To_GridLayout_And_Back));
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot(),
            testLog: log);

        // Wait for window to be ready, then inspect home page
        // The button somehow returns width of -6 initially as if window size was 0 the first layout.
        Thread.Sleep(500);

        var home = await app.WaitForWindowAsync();
        await app.ScreenshotAsync();

        var gridButton = home.FindById("GridLayout");
        Assert.NotNull(gridButton);

        // Navigate to Grid Layout
        await app.ClickAsync(gridButton.CenterX, gridButton.CenterY);

        // Wait for the grid page to render (polls until the element appears)
        var gridPage = await app.WaitForElementAsync("Basic fixed grid");
        await app.ScreenshotAsync();

        // The grid page should have scenario navigation buttons
        var basicGrid = gridPage.FindById("Basic fixed grid");
        Assert.NotNull(basicGrid);

        // Navigate back
        var back = gridPage.FindById("Back");
        Assert.NotNull(back);
        await app.ClickAsync(back.CenterX, back.CenterY);

        // Wait for the home page to reappear
        var backHome = await app.WaitForElementAsync("GridLayout");
        Assert.NotNull(backHome.FindById("GridLayout"));
    }

    [Fact]
    public async Task Can_Navigate_To_All_Pages_And_Back()
    {
        await using var log = new TestLog(nameof(Can_Navigate_To_All_Pages_And_Back));
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot(),
            testLog: log);

        // The button somehow returns width of -6 initially as if window size was 0 the first layout.
        Thread.Sleep(500);

        var home = await app.WaitForWindowAsync();
        await app.ScreenshotAsync();

        // All example pages on the home screen and an element id to verify each page loaded.
        // CanScreenshot is false for pages whose rendering crashes the SVG screenshot context
        // (e.g. 3D uses DrawImage which is not implemented in the SVG context).
        var pages = new (string ButtonId, string VerifyId, bool CanScreenshot)[]
        {
            ("TextMetrics", "Back", true),
            ("TextLayout", "Back", true),
            ("NestedStacks", "Back", true),
            ("GridLayout", "Basic fixed grid", true),
            ("ViewCollectionAlignment", "Back", true),
            ("AnimatedHeart", "Back", true),
            ("TextBox", "NameBox", true),
            ("CanvasTests", "FillRect", true),
            ("Layers", "BorderLayer", true),
            ("3D", "RotatingCube", false),
        };

        foreach (var (buttonId, verifyId, canScreenshot) in pages)
        {
            Thread.Sleep(150);

            // Navigate to the page
            var root = await app.WaitForElementAsync(buttonId);
            var button = root.FindById(buttonId);
            Assert.NotNull(button);
            await app.ClickAsync(button.CenterX, button.CenterY);

            Thread.Sleep(250);

            // Wait for the page to load
            var page = await app.WaitForElementAsync(verifyId);
            Assert.NotNull(page.FindById(verifyId));
            if (canScreenshot)
                await app.ScreenshotAsync();

            // Navigate back
            var back = page.FindById("Back");
            Assert.NotNull(back);
            await app.ClickAsync(back.CenterX, back.CenterY);

            // Wait for home page to reappear
            await app.WaitForElementAsync("TextMetrics");
        }
    }

    [Fact]
    public async Task Can_Navigate_Through_All_Canvas_Scenarios()
    {
        await using var log = new TestLog(nameof(Can_Navigate_Through_All_Canvas_Scenarios));
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot(),
            testLog: log);

        Thread.Sleep(500);

        var home = await app.WaitForWindowAsync();

        // Navigate to Canvas Tests page
        var canvasButton = home.FindById("CanvasTests");
        Assert.NotNull(canvasButton);
        await app.ClickAsync(canvasButton.CenterX, canvasButton.CenterY);

        var canvasPage = await app.WaitForElementAsync("FillRect");

        // BitmapFill and DrawImage crash the app because the SVG context doesn't support
        // image pattern fill / DrawImage. They trigger NotImplementedException during the
        // regular render cycle (not just screenshot), so they must be excluded entirely.
        var scenarios = new[]
        {
            "FillRect", "QuadraticCurve", "CubicCurve", "HeartCurve",
            "Arc", "ArcFlower", "Ellipse", "ArcTo", "ArcToFlower",
            "RoundRect", "LineCapJoin", "DashPattern", "FillRule",
            "PathContinuation", "Clip", "Transform", "Star",
            "GlobalAlpha",
        };

        foreach (var scenarioId in scenarios)
        {
            Thread.Sleep(250);

            var root = await app.WaitForElementAsync(scenarioId);
            var btn = root.FindById(scenarioId);
            Assert.NotNull(btn);
            await app.ClickAsync(btn.CenterX, btn.CenterY);

            Thread.Sleep(250);

            await app.ScreenshotAsync();
        }
    }

    [Fact]
    public async Task Can_Navigate_Through_All_3D_Scenarios()
    {
        await using var log = new TestLog(nameof(Can_Navigate_Through_All_3D_Scenarios));
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot(),
            testLog: log);

        Thread.Sleep(500);

        var home = await app.WaitForWindowAsync();

        // Navigate to 3D page
        var threeDButton = home.FindById("3D");
        Assert.NotNull(threeDButton);
        await app.ClickAsync(threeDButton.CenterX, threeDButton.CenterY);

        var threeDPage = await app.WaitForElementAsync("RotatingCube");

        var scenarios = new[]
        {
            "RotatingCube",
            "GPUHardwareCube",
        };

        foreach (var scenarioId in scenarios)
        {
            Thread.Sleep(250);

            var root = await app.WaitForElementAsync(scenarioId);
            var btn = root.FindById(scenarioId);
            Assert.NotNull(btn);
            await app.ClickAsync(btn.CenterX, btn.CenterY);

            Thread.Sleep(250);

            // No screenshot - 3D pages use DrawImage which crashes the SVG context
            await app.InspectAsync();
        }
    }
}
