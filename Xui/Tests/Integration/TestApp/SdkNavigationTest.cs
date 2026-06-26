using Xui.Apps.TestApp;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;
using System.Runtime.CompilerServices;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests that boot the real TestApp via the test platform,
/// render the home page, send mouse events to navigate, and capture SVG snapshots.
/// </summary>
public class SdkNavigationTest
{
    private static Size WindowSize = (600, 400);
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

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void HomePage_Renders(TestRuntimeVariant runtimeVariant)
    {
        using var app = CreateApp(runtimeVariant);
        AddTestAppIntro(app);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Render the app home page as the baseline state.");
        app.Snapshot("HomePage");
    }

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void Navigate_To_TextMetrics(TestRuntimeVariant runtimeVariant)
    {
        using var app = CreateApp(runtimeVariant);
        AddTestAppIntro(app);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Navigate from home to the TextMetrics example and capture hover/press transitions.");
        app.Snapshot("HomePage");

        var button = app.Window.RootView.FindViewById("TextMetrics");
        Assert.NotNull(button);

        app.MouseMove(button);
        app.MarkdownList(["Hover TextMetrics button", "Press TextMetrics button", "Release to navigate"]);
        app.Snapshot("Hover");
        app.MouseDown(button);
        app.Snapshot("Pressed");
        app.MouseUp(button);
        app.Snapshot("TextMetrics");
    }

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void Navigate_Through_All(TestRuntimeVariant runtimeVariant)
    {
        using var app = CreateApp(runtimeVariant);
        AddTestAppIntro(app);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Visit each example page from home, snapshot it, then return to home.");
        app.Snapshot("HomePage");

        string[] pages = ["TextMetrics", "TextLayout", "NestedStacks", "ViewCollectionAlignment", "AnimatedHeart", "TextBox"];

        foreach (var page in pages)
        {
            app.MarkdownHeading($"Navigate: {page}", level: 3);
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

    /// <summary>
    /// Bug: After navigating to an example and back, the button that was
    /// hovered before navigation still shows its hover effect.
    /// Views removed from the tree should receive a pointer-leave event.
    /// </summary>
    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void Pending_Hover_After_Navigation(TestRuntimeVariant runtimeVariant)
    {
        using var app = CreateApp(runtimeVariant);
        AddTestAppIntro(app);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Reproduce stale hover state after navigating to NestedStacks and back.");
        app.Snapshot("HomePage");

        // Hover and click the NestedStacks button
        var button = app.Window.RootView.FindViewById("NestedStacks");
        Assert.NotNull(button);
        app.MouseMove(button);
        app.Snapshot("HoverNestedStacks");
        app.MouseDown(button);
        app.Snapshot("PressedNestedStacks");
        app.MouseUp(button);

        app.Snapshot("NestedStacks");

        // Click back
        var back = app.Window.RootView.FindViewById("Back");
        Assert.NotNull(back);
        app.MouseMove(back);
        app.Snapshot("HoverBack");
        app.MouseDown(back);
        app.Snapshot("PressHoverBack");
        app.MouseUp(back);
        app.Snapshot("ClickedHomePage");

        // Bug: NestedStacks button still shows hover despite being re-created
        // Move mouse away from any button to a neutral position
        app.MouseMove(new Point(10, 10));
        app.MarkdownCode("""
            Expected: returning home should clear hover state from removed views.
            Validation: move mouse to a neutral position and snapshot visual state.
            """);
        app.Snapshot("MouseMovedAway");
    }

    /// <summary>
    /// Bug: After moving the mouse over the back button (without clicking),
    /// the animated heart stops animating — animation frames no longer
    /// produce different renders at rest vs peak times.
    /// </summary>
    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void Heartbeat_Stops_After_Mouse_Over_Back(TestRuntimeVariant runtimeVariant)
    {
        using var app = CreateApp(runtimeVariant);
        AddTestAppIntro(app);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Verify that heart animation keeps ticking after pointer hover on Back.");

        // Navigate to AnimatedHeart
        var button = app.Window.RootView.FindViewById("AnimatedHeart");
        app.Snapshot("Home");

        Assert.NotNull(button);
        app.MouseMove(button);
        app.MouseDown(button);
        app.MouseUp(button);

        // Capture heart at rest (t=0.0s) and primary peak (t=0.10s)
        app.AnimationFrame(TimeSpan.Zero, TimeSpan.Zero);
        app.Snapshot("Heart.Rest");
        app.AnimationFrame(TimeSpan.Zero, TimeSpan.FromSeconds(0.10));
        app.Snapshot("Heart.PrimaryPeak");

        // Move mouse over the back button (do NOT click)
        var back = app.Window.RootView.FindViewById("Back");
        Assert.NotNull(back);
        app.MouseMove(back);
        app.Snapshot("MouseOverBack");

        // Bug: heart should still animate but it stops
        // Capture the same two phases again
        app.AnimationFrame(TimeSpan.FromSeconds(0.10), TimeSpan.FromSeconds(0.833));
        app.Snapshot("Heart.Rest.AfterMouseOver");
        app.AnimationFrame(TimeSpan.FromSeconds(0.833), TimeSpan.FromSeconds(0.933));
        app.MarkdownList(
        [
            "Heart.Rest vs Heart.PrimaryPeak should differ before hover.",
            "Heart.Rest.AfterMouseOver vs Heart.PrimaryPeak.AfterMouseOver should also differ."
        ]);
        app.Snapshot("Heart.PrimaryPeak.AfterMouseOver");
    }
}
