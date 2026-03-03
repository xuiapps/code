using System.Runtime.CompilerServices;
using Xui.Apps.BlankApp;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests for the <see cref="Xui.SDK.UI.PatternBox"/> component.
/// Navigates to the Pattern Input demo page and exercises slot clicks,
/// character typing, backspace, and cursor navigation.
/// </summary>
public class PatternBoxTest
{
    private static Size WindowSize = (600, 400);

    private static TestSinglePageApp<Application, MainWindow> NavigateToPatternBox(
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string testName = "")
    {
        var app = new TestSinglePageApp<Application, MainWindow>(WindowSize, callerPath: callerPath, testName: testName);

        app.Render();

        var home = app.Window.RootView.FindViewById("InputWidgets");
        app.MouseMove(home!);
        app.MouseDown(home!);
        app.MouseUp(home!);

        app.Render();

        // Navigate to Pattern Input sub-page
        var patternBtn = app.Window.RootView.FindViewById("PatternInput");
        if (patternBtn is not null)
        {
            app.MouseDown(patternBtn);
            app.MouseUp(patternBtn);
            app.Render();
        }

        return app;
    }

    [Fact]
    public void PatternBox_Renders()
    {
        using var app = NavigateToPatternBox();
        app.Snapshot("PatternBoxPage");
    }

    [Fact]
    public void PatternBox_Focus()
    {
        using var app = NavigateToPatternBox();

        var box = app.Window.RootView.FindViewById("PinBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);
        app.Snapshot("PinFocused");
    }

    [Fact]
    public void PatternBox_TypePin()
    {
        using var app = NavigateToPatternBox();

        var box = app.Window.RootView.FindViewById("PinBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        app.Type("123456");
        app.Snapshot("PinTyped_123456");
    }

    [Fact]
    public void PatternBox_Backspace()
    {
        using var app = NavigateToPatternBox();

        var box = app.Window.RootView.FindViewById("PinBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        // Type 5 of 6 digits — stays focused in PinBox
        app.Type("12345");
        app.Snapshot("BeforeBackspace");

        app.KeyDown(VirtualKey.Back);
        app.KeyDown(VirtualKey.Back);
        app.Snapshot("AfterBackspace");
    }

    [Fact]
    public void PatternBox_AutoAdvanceFocus()
    {
        using var app = NavigateToPatternBox();

        var pinBox = app.Window.RootView.FindViewById("PinBox");
        var cardBox = app.Window.RootView.FindViewById("CardBox");
        Assert.NotNull(pinBox);
        Assert.NotNull(cardBox);

        app.MouseDown(pinBox);
        app.MouseUp(pinBox);

        // Type all 6 digits — last digit auto-advances focus to CardBox
        app.Type("123456");
        app.Render();

        Assert.Equal(cardBox, app.Window.RootView.FocusedView);
        app.Snapshot("FocusAdvancedToCard");
    }

    [Fact]
    public void PatternBox_ArrowNavigation()
    {
        using var app = NavigateToPatternBox();

        var box = app.Window.RootView.FindViewById("PinBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        app.Type("1234");
        app.Snapshot("Typed_1234");

        // Move left
        app.KeyDown(VirtualKey.Left);
        app.KeyDown(VirtualKey.Left);
        app.Snapshot("MovedLeft");

        // Type replaces at cursor position
        app.Type("9");
        app.Snapshot("Replaced");
    }

    [Fact]
    public void PatternBox_CreditCard()
    {
        using var app = NavigateToPatternBox();

        var box = app.Window.RootView.FindViewById("CardBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        app.Type("4111111111111111");
        app.Snapshot("CardTyped");
    }
}
