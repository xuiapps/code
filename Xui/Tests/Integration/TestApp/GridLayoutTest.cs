using Xui.Apps.BlankApp;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests that navigate to the Grid Layout example and snapshot
/// each of the individual grid scenarios.
/// </summary>
public class GridLayoutTest
{
    private static Size WindowSize = (800, 560);

    private static readonly string[] Scenarios =
    [
        "Basic fixed grid",
        "Grid with gaps",
        "Fractional units",
        "Auto tracks",
        "Spanning cells",
        "Named areas",
        "Auto flow",
        "Alignment",
        "Fixed container",
        "Scrollable grid",
        "Nested grids",
        "MinMax tracks",
    ];

    /// <summary>
    /// Navigates to the Grid Layout example and snapshots each scenario in sequence.
    /// </summary>
    [Fact]
    public void Grid_Scenarios()
    {
        using var app = new TestSinglePageApp<Application, MainWindow>(WindowSize);

        // Render first so the home page is laid out and buttons are hit-testable
        app.Render();

        // Navigate to Grid Layout
        var gridButton = app.Window.RootView.FindViewById("GridLayout");
        Assert.NotNull(gridButton);
        app.MouseMove(gridButton);
        app.MouseDown(gridButton);
        app.MouseUp(gridButton);
        app.Render();
        app.Snapshot("GridLayout.Home");

        // Snapshot each scenario by clicking its nav button
        foreach (var scenario in Scenarios)
        {
            var navButton = app.Window.RootView.FindViewById(scenario);
            Assert.NotNull(navButton);
            app.MouseMove(navButton);
            app.MouseDown(navButton);
            app.MouseUp(navButton);
            app.Snapshot(scenario.Replace(" ", "."));
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
