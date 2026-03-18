using Xui.Apps.BlankApp;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests that navigate to the FlexBox Layout example and snapshot
/// each of the individual flexbox scenarios.
/// </summary>
public class FlexBoxLayoutTest
{
    private static Size WindowSize = (800, 560);

    private static readonly string[] Scenarios =
    [
        "Basic row",
        "Basic column",
        "Grow & shrink",
        "Wrapping",
        "Justify content",
        "Align items",
        "Align content",
        "Gaps",
        "Reverse direction",
        "Nested flex",
        "Responsive layout",
        "Mixed sizing",
    ];

    /// <summary>
    /// Navigates to the FlexBox Layout example and snapshots each scenario in sequence.
    /// </summary>
    [Fact]
    public void FlexBox_Scenarios()
    {
        using var app = new TestSinglePageApp<Application, MainWindow>(WindowSize);

        // Render first so the home page is laid out and buttons are hit-testable
        app.Render();

        // Navigate to FlexBox Layout
        var flexBoxButton = app.Window.RootView.FindViewById("FlexBoxLayout");
        Assert.NotNull(flexBoxButton);
        app.MouseMove(flexBoxButton);
        app.MouseDown(flexBoxButton);
        app.MouseUp(flexBoxButton);
        app.Render();
        app.Snapshot("FlexBoxLayout.Home");

        // Snapshot each scenario by clicking its nav button
        foreach (var scenario in Scenarios)
        {
            var navButton = app.Window.RootView.FindViewById(scenario);
            Assert.NotNull(navButton);
            app.MouseMove(navButton);
            app.MouseDown(navButton);
            app.MouseUp(navButton);
            app.Snapshot(scenario.Replace(" ", ".").Replace("&", "and"));
        }

        // Navigate back home
        var back = app.Window.RootView.FindViewById("Back");
        Assert.NotNull(back);
        app.MouseMove(back);
        app.MouseDown(back);
        app.MouseUp(back);
        app.Snapshot("HomePage");
    }
}
