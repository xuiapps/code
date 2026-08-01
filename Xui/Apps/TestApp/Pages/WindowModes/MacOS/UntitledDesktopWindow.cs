using Xui.Core.Abstract;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>A desktop window whose Xui client area includes the native title-bar region.</summary>
public sealed class UntitledDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 320);

    public UntitledDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled";
        Content = new Border
        {
            BorderThickness = 1,
            BorderColor = Colors.Gray,
            Margin = 10,
            Content = new VerticalStack
            {
                Margin = 10,
                Content = [
                    new Label { Text = "Untitled", Font = new(20, "Inter", Core.Canvas.FontWeight.Bold), Margin = (0, 0, 8, 0) },
                    new Label { Text = "Xui draws through the title-bar region; the native title text is hidden.", FontFamily = "Inter" },
                ],
            },
        };
    }
}
