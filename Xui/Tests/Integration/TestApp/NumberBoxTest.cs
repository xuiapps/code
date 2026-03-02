using System.Runtime.CompilerServices;
using Xui.Apps.BlankApp;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests for the <see cref="Xui.SDK.UI.NumberBox"/> component.
/// Navigates to the Number Input demo page and exercises increment/decrement buttons,
/// keyboard arrows, digit typing, and focus/blur.
/// </summary>
public class NumberBoxTest
{
    private static Size WindowSize = (600, 400);

    private static TestSinglePageApp<Application, MainWindow> NavigateToNumberBox(
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string testName = "")
    {
        var app = new TestSinglePageApp<Application, MainWindow>(WindowSize, callerPath: callerPath, testName: testName);

        app.Render();

        // Navigate to Input Widgets page
        var button = app.Window.RootView.FindViewById("InputWidgets");
        app.MouseMove(button!);
        app.MouseDown(button!);
        app.MouseUp(button!);

        app.Render();
        return app;
    }

    [Fact]
    public void NumberBox_Renders()
    {
        using var app = NavigateToNumberBox();
        app.Snapshot("NumberBoxPage");
    }

    [Fact]
    public void NumberBox_Focus()
    {
        using var app = NavigateToNumberBox();

        var box = app.Window.RootView.FindViewById("DefaultNumberBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);
        app.Snapshot("Focused");
    }

    [Fact]
    public void NumberBox_KeyboardArrows()
    {
        using var app = NavigateToNumberBox();

        // Focus the default box
        var box = app.Window.RootView.FindViewById("DefaultNumberBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);
        app.Snapshot("Focused");

        // Arrow Up increments
        app.KeyDown(VirtualKey.Up);
        app.Snapshot("ArrowUp_1");
        app.KeyDown(VirtualKey.Up);
        app.KeyDown(VirtualKey.Up);
        app.Snapshot("ArrowUp_3");

        // Arrow Down decrements
        app.KeyDown(VirtualKey.Down);
        app.Snapshot("ArrowDown");
    }

    [Fact]
    public void NumberBox_IncrementButton()
    {
        using var app = NavigateToNumberBox();

        var box = app.Window.RootView.FindViewById("DefaultNumberBox");
        Assert.NotNull(box);
        app.Render();

        // Click the right (+) button — it's at Frame.Right - border - buttonWidth/2
        var frame = box.Frame;
        var plusX = frame.Right - 16; // roughly center of right 32px button
        var plusY = frame.Y + frame.Height / 2;
        var plusPt = new Point(plusX, plusY);

        app.MouseDown(plusPt);
        app.Snapshot("PlusPressed");
        app.MouseUp(plusPt);
        app.Snapshot("PlusReleased");
    }

    [Fact]
    public void NumberBox_DecrementButton()
    {
        using var app = NavigateToNumberBox();

        var box = app.Window.RootView.FindViewById("DefaultNumberBox");
        Assert.NotNull(box);
        app.Render();

        // Focus first so it has an initial value, then click minus
        app.MouseDown(box);
        app.MouseUp(box);

        var frame = box.Frame;
        var minusX = frame.X + 16; // roughly center of left 32px button
        var minusY = frame.Y + frame.Height / 2;
        var minusPt = new Point(minusX, minusY);

        app.MouseDown(minusPt);
        app.Snapshot("MinusPressed");
        app.MouseUp(minusPt);
        app.Snapshot("MinusReleased");
    }

    [Fact]
    public void NumberBox_TypeDigit()
    {
        using var app = NavigateToNumberBox();

        var box = app.Window.RootView.FindViewById("DefaultNumberBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        app.Type("42");
        app.Snapshot("Typed_42");

        // Blur by clicking elsewhere — should commit the value
        app.MouseDown(new Point(10, 10));
        app.MouseUp(new Point(10, 10));
        app.Snapshot("Blurred");
    }

    [Fact]
    public void NumberBox_BoundedAtMin()
    {
        using var app = NavigateToNumberBox();

        var box = app.Window.RootView.FindViewById("BoundedAtMinBox");
        Assert.NotNull(box);
        app.Render();

        // The − button should be disabled (value is at Min = −5)
        app.MouseDown(box);
        app.MouseUp(box);
        app.Snapshot("AtMin");

        // Arrow Down at min should be a no-op
        app.KeyDown(VirtualKey.Down);
        app.Snapshot("AtMin_ArrowDown");
    }
}
