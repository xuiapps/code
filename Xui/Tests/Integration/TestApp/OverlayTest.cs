using Xui.Apps.TestApp;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;
using System.Runtime.CompilerServices;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests for the IOverlay cross-platform in-window overlay.
/// Navigates to the Overlay demo, opens an overlay anchored below a button,
/// and verifies dismiss-on-outside-click behavior via SVG snapshots.
/// </summary>
public class OverlayTest
{
    private static readonly Size WindowSize = (800, 560);
    private static TestSinglePageApp<Application, MainWindow> CreateApp(
        TestRuntimeVariant runtimeVariant,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string testName = "") =>
        IntegrationRuntimeVariants.CreateApp<Application, MainWindow>(
            WindowSize, runtimeVariant, callerPath, testName);

    private static void AddTestAppIntro(TestSinglePageApp<Application, MainWindow> app)
    {
        app.MarkdownHeading("TestApp", level: 1);
        app.MarkdownParagraph("Open the test app to see the menu with SDK examples.");
    }

    // The "Open Overlay" button is centered in the right preview pane.
    // Coordinates derived from snapshot: button rect is (420, 244, 160, 36), center = (500, 262).
    private static readonly Point OverlayButtonCenter = (500, 262);

    // A point clearly outside the overlay (top-left of the preview pane).
    private static readonly Point OutsideOverlay = (220, 40);

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void Overlay_OpenAndDismiss(TestRuntimeVariant runtimeVariant)
    {
        using var app = CreateApp(runtimeVariant);
        AddTestAppIntro(app);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Open the Layers overlay demo, display overlay popup, and dismiss by outside click.");
        app.MarkdownCode("""
            Interaction points:
            - Open Overlay button: (500, 262)
            - Outside click dismiss point: (220, 40)
            """);

        // Navigate to Layers section
        app.Render();
        var layersBtn = app.Window.RootView.FindViewById("Layers");
        Assert.NotNull(layersBtn);
        app.MouseMove(layersBtn);
        app.MouseDown(layersBtn);
        app.MouseUp(layersBtn);
        app.Render();

        // Select the Overlay (IOverlay) demo in the layers panel
        var overlayBtn = app.Window.RootView.FindViewById("Overlay (IOverlay)");
        Assert.NotNull(overlayBtn);
        app.MouseMove(overlayBtn);
        app.MouseDown(overlayBtn);
        app.MouseUp(overlayBtn);
        app.Render();
        app.Snapshot("Layers.OverlayDemo");

        // Click the "Open Overlay" button in the demo to show the overlay
        app.MouseMove(OverlayButtonCenter);
        app.MouseDown(OverlayButtonCenter);
        app.MouseUp(OverlayButtonCenter);
        app.Render();
        app.Snapshot("Layers.OverlayOpen");

        // Click outside the overlay (top-left of preview pane) to dismiss it
        app.MouseMove(OutsideOverlay);
        app.MouseDown(OutsideOverlay);
        app.MouseUp(OutsideOverlay);
        app.Render();
        app.Snapshot("Layers.OverlayDismissed");
    }
}
