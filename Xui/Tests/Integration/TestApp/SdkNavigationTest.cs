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
    private static Size WindowSize = (600, 400);

    [Fact]
    public void HomePage_Renders()
    {
        using var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), WindowSize);
        app.Snapshot("HomePage");
    }

    [Fact]
    public void Navigate_To_TextMetrics()
    {
        using var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), WindowSize);
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

    [Fact]
    public void Navigate_Through_All()
    {
        using var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), WindowSize);
        app.Snapshot("HomePage");

        string[] pages = ["TextMetrics", "TextLayout", "NestedStacks", "ViewCollectionAlignment", "AnimatedHeart"];

        foreach (var page in pages)
        {
            var button = app.Window.RootView.FindViewById(page);
            Assert.NotNull(button);
            app.MouseMove(button);
            app.MouseDown(button);
            app.MouseUp(button);

            app.AnimationFrame(TimeSpan.Zero, TimeSpan.Zero);
            app.Snapshot(page);

            var back = app.Window.RootView.FindViewById("Back");
            Assert.NotNull(back);
            app.MouseMove(back);
            app.MouseDown(back);
            app.MouseUp(back);
        }
    }
}
