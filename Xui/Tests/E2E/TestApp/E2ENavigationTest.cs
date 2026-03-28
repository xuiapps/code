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
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot());

        var root = await app.InspectAsync();

        Assert.NotNull(root);

        // The home page should have navigation buttons for the examples.
        var gridButton = root.FindById("GridLayout");
        Assert.NotNull(gridButton);
        Assert.True(gridButton.Visible);
    }

    [Fact]
    public async Task Can_Navigate_To_GridLayout_And_Back()
    {
        await using var app = await RealApp.StartAsync(
            TestAppProject,
            workingDirectory: SolutionRoot());

        // Inspect home page
        var home = await app.InspectAsync();
        Assert.NotNull(home);

        var gridButton = home.FindById("GridLayout");
        Assert.NotNull(gridButton);

        // Navigate to Grid Layout
        await app.ClickAsync(gridButton.CenterX, gridButton.CenterY);

        // Give the app time to render the new page
        await Task.Delay(300);

        var gridPage = await app.InspectAsync();
        Assert.NotNull(gridPage);

        // The grid page should have scenario navigation buttons
        var basicGrid = gridPage.FindById("Basic fixed grid");
        Assert.NotNull(basicGrid);

        // Navigate back
        var back = gridPage.FindById("Back");
        Assert.NotNull(back);
        await app.ClickAsync(back.CenterX, back.CenterY);

        await Task.Delay(200);

        // Home page should be visible again
        var backHome = await app.InspectAsync();
        Assert.NotNull(backHome);
        Assert.NotNull(backHome.FindById("GridLayout"));
    }
}
