using System.Runtime.CompilerServices;
using Xui.Apps.BlankApp;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests for the <see cref="Xui.SDK.UI.CurrencyBox"/> component.
/// Navigates to the Currency Input demo page and exercises focus, typing,
/// backspace, blur formatting, and the overflow hint.
/// </summary>
public class CurrencyBoxTest
{
    private static Size WindowSize = (600, 400);

    private static TestSinglePageApp<Application, MainWindow> NavigateToCurrencyBox(
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

        // Navigate to Currency Input sub-page
        var currBtn = app.Window.RootView.FindViewById("CurrencyInput");
        if (currBtn is not null)
        {
            app.MouseDown(currBtn);
            app.MouseUp(currBtn);
            app.Render();
        }

        return app;
    }

    [Fact]
    public void CurrencyBox_Renders()
    {
        using var app = NavigateToCurrencyBox();
        app.Snapshot("CurrencyBoxPage");
    }

    [Fact]
    public void CurrencyBox_Focus()
    {
        using var app = NavigateToCurrencyBox();

        var box = app.Window.RootView.FindViewById("UsdBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);
        app.Snapshot("UsdFocused");
    }

    [Fact]
    public void CurrencyBox_TypeAndBlur()
    {
        using var app = NavigateToCurrencyBox();

        var box = app.Window.RootView.FindViewById("UsdBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        app.Type("9999.99");
        app.Snapshot("Typed");

        // Blur by clicking elsewhere — should format the value
        app.MouseDown(new Point(10, 10));
        app.MouseUp(new Point(10, 10));
        app.Snapshot("Blurred_Formatted");
    }

    [Fact]
    public void CurrencyBox_Backspace()
    {
        using var app = NavigateToCurrencyBox();

        var box = app.Window.RootView.FindViewById("UsdBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);

        app.Type("1234.56");
        app.Snapshot("BeforeBackspace");

        app.KeyDown(VirtualKey.Back);
        app.KeyDown(VirtualKey.Back);
        app.Snapshot("AfterBackspace");
    }

    [Fact]
    public void CurrencyBox_EurSuffix()
    {
        using var app = NavigateToCurrencyBox();

        var box = app.Window.RootView.FindViewById("EurBox");
        Assert.NotNull(box);
        app.MouseDown(box);
        app.MouseUp(box);
        app.Snapshot("EurFocused");

        app.Type("500");
        app.Snapshot("EurTyped");

        app.MouseDown(new Point(10, 10));
        app.MouseUp(new Point(10, 10));
        app.Snapshot("EurBlurred");
    }

    [Fact]
    public void CurrencyBox_LargeValueOverflow()
    {
        using var app = NavigateToCurrencyBox();

        // The large value box should render overflow hint
        var box = app.Window.RootView.FindViewById("LargeBox");
        Assert.NotNull(box);
        app.Snapshot("LargeValue_Overflow");
    }
}
