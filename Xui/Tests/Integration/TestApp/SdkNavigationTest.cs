using System.Runtime.CompilerServices;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests that boot the real TestApp via the test platform,
/// render the home page, send mouse events to navigate, and capture SVG snapshots.
/// </summary>
public class SdkNavigationTest
{
    [Fact]
    public void HomePage_Renders()
    {
        using var app = new Xui.Runtime.Test.TestSinglePageApp(new Xui.Apps.BlankApp.App(), new Size(400, 700));
        var svg = app.Render();

        var expectedPath = GetSnapshotPath("Snapshots/HomePage.svg");
        var actualPath = GetSnapshotPath("Snapshots/HomePage.Actual.svg");

        AssertSnapshot(expectedPath, actualPath, svg);
    }

    [Fact]
    public void Navigate_To_TextMetrics()
    {
        using var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), new Size(400, 700));

        // Initial render to lay out the home page
        app.Render();

        // Find the TextMetrics button by Id and click it
        var button = app.Window.RootView.FindViewById("TextMetrics");
        Assert.NotNull(button);

        app.MouseMove(button);
        app.Render();
        app.MouseDown(button);
        app.Render();
        app.MouseUp(button);
        var svg = app.Render(); // now on TextMetrics example page

        var expectedPath = GetSnapshotPath("Snapshots/TextMetrics.svg");
        var actualPath = GetSnapshotPath("Snapshots/TextMetrics.Actual.svg");

        AssertSnapshot(expectedPath, actualPath, svg);
    }

    private static void AssertSnapshot(string expectedPath, string actualPath, string actual)
    {
        string expected;
        try
        {
            expected = File.ReadAllText(expectedPath);
        }
        catch
        {
            expected = "";
        }

        try
        {
            Assert.Equal(expected, actual);
        }
        catch
        {
            File.WriteAllText(actualPath, actual);
            throw;
        }
    }

    private static string GetSnapshotPath(string fileName, [CallerFilePath] string callerPath = "")
    {
        var sourceDir = Path.GetDirectoryName(callerPath)!;
        return Path.Combine(sourceDir, fileName);
    }
}
