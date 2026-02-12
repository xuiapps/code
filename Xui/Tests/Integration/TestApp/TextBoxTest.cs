using System.Runtime.CompilerServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

/// <summary>
/// Integration tests for the TextBox component.
/// Navigates to the TextBox example and exercises focus, typing, backspace, and password masking.
/// </summary>
public class TextBoxTest
{
    private static Size WindowSize = (600, 400);

    private static TestSinglePageApp NavigateToTextBox(
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string testName = "")
    {
        var app = new TestSinglePageApp(new Xui.Apps.BlankApp.App(), WindowSize, callerPath, testName);

        var button = app.Window.RootView.FindViewById("TextBox");
        app.MouseMove(button!);
        app.MouseDown(button!);
        app.MouseUp(button!);

        // Render so the new page's views get measured/arranged (have valid Frames)
        app.Render();

        return app;
    }

    [Fact]
    public void TextBox_Focus()
    {
        using var app = NavigateToTextBox();
        app.Snapshot("TextBoxPage");

        // Click the Name TextBox to focus it
        var nameBox = app.Window.RootView.FindViewById("NameBox");
        Assert.NotNull(nameBox);
        app.MouseDown(nameBox);
        app.MouseUp(nameBox);

        app.Snapshot("NameBoxFocused");
    }

    [Fact]
    public void TextBox_Type()
    {
        using var app = NavigateToTextBox();

        // Click the Name TextBox to focus it
        var nameBox = app.Window.RootView.FindViewById("NameBox");
        Assert.NotNull(nameBox);
        app.MouseDown(nameBox);
        app.MouseUp(nameBox);

        // Type text
        app.Type("Hello");
        app.Snapshot("Typed.Hello");

        // Continue typing
        app.Type(" World");
        app.Snapshot("Typed.HelloWorld");
    }

    [Fact]
    public void TextBox_Backspace()
    {
        using var app = NavigateToTextBox();

        // Click the Name TextBox to focus it
        var nameBox = app.Window.RootView.FindViewById("NameBox");
        Assert.NotNull(nameBox);
        app.MouseDown(nameBox);
        app.MouseUp(nameBox);

        // Type text then delete some
        app.Type("Hello");
        app.Snapshot("BeforeBackspace");

        app.KeyDown(VirtualKey.Back);
        app.KeyDown(VirtualKey.Back);
        app.Snapshot("AfterBackspace");

        // Type more after deletion
        app.Type("p");
        app.Snapshot("AfterRetype");
    }

    [Fact]
    public void TextBox_Password()
    {
        using var app = NavigateToTextBox();

        // Click the Password TextBox to focus it
        var passwordBox = app.Window.RootView.FindViewById("PasswordBox");
        Assert.NotNull(passwordBox);
        app.MouseDown(passwordBox);
        app.MouseUp(passwordBox);

        // Type a password — should render as bullet characters
        app.Type("secret");
        app.Snapshot("PasswordTyped");
    }

    [Fact]
    public void TextBox_SwitchFocus()
    {
        using var app = NavigateToTextBox();

        // Focus and type in the Name box
        var nameBox = app.Window.RootView.FindViewById("NameBox");
        Assert.NotNull(nameBox);
        app.MouseDown(nameBox);
        app.MouseUp(nameBox);
        app.Type("Alice");
        app.Snapshot("NameFilled");

        // Click the Password box — focus should move
        var passwordBox = app.Window.RootView.FindViewById("PasswordBox");
        Assert.NotNull(passwordBox);
        app.MouseDown(passwordBox);
        app.MouseUp(passwordBox);
        app.Snapshot("PasswordFocused");

        // Type in password box
        app.Type("pass");
        app.Snapshot("PasswordFilled");
    }

    [Fact]
    public void TextBox_TabNavigation()
    {
        using var app = NavigateToTextBox();

        // Tab into the first TextBox (NameBox)
        app.KeyDown(VirtualKey.Tab);
        app.Type("Tab1");
        app.Snapshot("Tab.NameBox");

        // Tab to the second TextBox (PasswordBox)
        app.KeyDown(VirtualKey.Tab);
        app.Type("pass");
        app.Snapshot("Tab.PasswordBox");

        // Tab to NumberBox
        app.KeyDown(VirtualKey.Tab);
        app.Snapshot("Tab.NumberBox");

        // Tab to ColorBox
        app.KeyDown(VirtualKey.Tab);
        app.Snapshot("Tab.ColorBox");

        // Tab wraps back to NameBox
        app.KeyDown(VirtualKey.Tab);
        app.Snapshot("Tab.WrapToNameBox");

        // Shift+Tab goes back to ColorBox
        app.KeyDown(VirtualKey.Tab, shift: true);
        app.Snapshot("ShiftTab.ColorBox");
    }
}
