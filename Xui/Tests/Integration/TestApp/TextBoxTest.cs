using System.Runtime.CompilerServices;
using Xui.Apps.TestApp;
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

    private static void AddTestAppIntro(TestSinglePageApp<Application, MainWindow> app)
    {
        app.MarkdownHeading("TestApp", level: 1);
        app.MarkdownParagraph("Open the test app to see the menu with SDK examples.");
    }

    private static TestSinglePageApp<Application, MainWindow> NavigateToTextBox(
        TestRuntimeVariant runtimeVariant,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string testName = "")
    {
        var app = IntegrationRuntimeVariants.CreateApp<Application, MainWindow>(
            WindowSize, runtimeVariant, callerPath, testName);
        AddTestAppIntro(app);

        // Render first so all home page buttons have valid Frames for hit-testing.
        // Without this, all views have zero frames and the hit-test hits the last
        // button in the list (reverse-order traversal) rather than the intended one.
        app.Render();

        var button = app.Window.RootView.FindViewById("TextBox");
        app.MouseMove(button!);
        app.MouseDown(button!);
        app.MouseUp(button!);

        // Render so the new page's views get measured/arranged (have valid Frames)
        app.Render();

        return app;
    }

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_Focus(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Focus the Name text box and verify focus visuals.");
        app.Snapshot("TextBoxPage");

        // Click the Name TextBox to focus it
        var nameBox = app.Window.RootView.FindViewById("NameBox");
        Assert.NotNull(nameBox);
        app.MouseDown(nameBox);
        app.MouseUp(nameBox);

        app.Snapshot("NameBoxFocused");
    }

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_Type(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Type incrementally in NameBox and verify content growth.");

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

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_Backspace(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Type text, backspace twice, then type replacement character.");

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

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_Password(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Type into PasswordBox and verify masked rendering.");

        // Click the Password TextBox to focus it
        var passwordBox = app.Window.RootView.FindViewById("PasswordBox");
        Assert.NotNull(passwordBox);
        app.MouseDown(passwordBox);
        app.MouseUp(passwordBox);

        // Type a password — should render as bullet characters
        app.Type("secret");
        app.Snapshot("PasswordTyped");
    }

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_SwitchFocus(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Move focus from NameBox to PasswordBox and verify independent values.");

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

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_TabNavigation(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Traverse editable controls with Tab and Shift+Tab.");
        app.MarkdownList(
        [
            "Tab to NameBox, then PasswordBox, NumberBox, ColorBox.",
            "Tab wraps to NameBox.",
            "Shift+Tab moves focus backward."
        ]);

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

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_KeyboardSelection(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Use keyboard selection and replacement inside NameBox.");

        // Tab to focus the first TextBox (NameBox)
        app.KeyDown(VirtualKey.Tab);

        // Type "Hello World!"
        app.Type("Hello World!");
        app.Snapshot("Typed");

        // Move caret left twice: cursor lands before "d!"
        app.KeyDown(VirtualKey.Left);
        app.KeyDown(VirtualKey.Left);
        app.Snapshot("AfterLeftLeft");

        // Shift+Left twice to select the two characters before the cursor ("l" and "r")
        app.KeyDown(VirtualKey.Left, shift: true);
        app.KeyDown(VirtualKey.Left, shift: true);
        app.Snapshot("ShiftSelected");

        // Type "yep" to replace the selection
        app.Type("yep");
        app.Snapshot("AfterReplace");
    }

    [Theory]
    [MemberData(nameof(IntegrationRuntimeVariants.All), MemberType = typeof(IntegrationRuntimeVariants))]
    public void TextBox_MouseSelection(TestRuntimeVariant runtimeVariant)
    {
        using var app = NavigateToTextBox(runtimeVariant);
        app.MarkdownHeading("Scenario");
        app.MarkdownParagraph("Drag-select text range in NameBox with mouse.");

        // Tab to focus NameBox and type some text
        app.KeyDown(VirtualKey.Tab);
        app.Type("Hello World!");
        app.Render();

        var nameBox = app.Window.RootView.FindViewById("NameBox");
        Assert.NotNull(nameBox);

        var origin = nameBox.Frame.TopLeft;
        var start = new Point(origin.X + 20, origin.Y + 10);
        var end = new Point(start.X + 20, start.Y);

        // Mouse down at start position, drag to end position, release
        app.MouseDown(start);
        app.Snapshot("MouseDown");

        app.MouseMove(end);
        app.Snapshot("MouseDrag");

        app.MouseUp(end);
        app.Snapshot("MouseUp");
    }
}
