using Xui.Core.Abstract;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// A standard, system-decorated desktop window. The native platform owns the
/// title bar, including the centered macOS title and system caption controls.
/// </summary>
public sealed class StandardDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 320);
    WindowClientArea IWindow.IDesktopStyle.ClientArea => WindowClientArea.Default;

    public StandardDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Normal Desktop Window";
        Content = new Border()
        {
            BorderThickness = 1,
            BorderColor = Colors.Gray,
            Margin = 10,
            Content = new VerticalStack
            {
                Margin = 10,
                Content = [
                    new Label
                    {
                        Text = "Normal desktop window",
                        Font = new(20, "Inter", Core.Canvas.FontWeight.Bold),
                        Margin = (0, 0, 8, 0),
                    },
                    new Label
                    {
                        Text = "This uses the platform's standard title bar and system window buttons.",
                        FontFamily = "Inter",
                    },
                ]
            }
        };
    }
}
