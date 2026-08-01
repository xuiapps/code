using Xui.Core.Abstract;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>A desktop window with a hidden title and unified native toolbar.</summary>
public sealed class UntitledUnifiedDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public UntitledUnifiedDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled Unified";
        Content = CreateContent("Untitled unified", "A visible unified native toolbar expands the title region.");
    }

    internal static Border CreateContent(string heading, string description) => new()
    {
        BorderThickness = 1,
        BorderColor = Colors.Gray,
        Margin = 10,
        Content = new VerticalStack
        {
            Margin = 10,
            Content = [
                new Label { Text = heading, Font = new(20, "Inter", Core.Canvas.FontWeight.Bold), Margin = (0, 0, 8, 0) },
                new Label { Text = description, FontFamily = "Inter" },
            ],
        },
    };
}
