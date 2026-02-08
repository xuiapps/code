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
        using var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), new Size(400, 700));
        app.Snapshot("HomePage");
    }

    [Fact]
    public void Navigate_To_TextMetrics()
    {
        using var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), new Size(400, 700));
        app.Snapshot("HomePage");

        var button = app.Window.RootView.FindViewById("TextMetrics");
        Assert.NotNull(button);

        app.MouseMove(button);
        app.Snapshot("Hover");
        app.MouseDown(button);
        app.Snapshot("Pressed");
        app.MouseUp(button);
        app.Snapshot("TextMetrics");
    }
}
