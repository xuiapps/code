using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// Shared Xui-drawn frame for transparent native windows.
/// </summary>
public abstract class TransparentWindow : Window
{
    private static readonly NFloat BorderSize = 4;
    private static readonly NFloat HeaderHeight = 48;

    protected TransparentWindow(IServiceProvider context, string heading, string description) : base(context)
    {
        Content = new Border
        {
            Margin = 2,
            BorderThickness = BorderSize,
            BorderColor = Colors.Black,
            CornerRadius = 25,
            BackgroundColor = Colors.White,
            Content = new VerticalStack
            {
                Content =
                [
                    new Border
                    {
                        MinimumHeight = HeaderHeight,
                        BorderThickness = (0, 0, 1, 0),
                        BorderColor = Colors.Black,
                        Content = new Label
                        {
                            Text = heading,
                            Font = new Font(15, "Inter", FontWeight.SemiBold),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Middle,
                        },
                    },
                    new VerticalStack
                    {
                        Margin = 20,
                        Content =
                        [
                            new Label
                            {
                                Text = heading,
                                Font = new Font(18, "Inter", FontWeight.SemiBold),
                                Margin = (0, 0, 8, 0),
                            },
                            new Label
                            {
                                Text = description,
                                FontFamily = "Inter",
                            },
                        ],
                    },
                ],
            },
        };
    }

}

/// <summary>
/// A transparent native window with a rounded Xui-drawn frame.
/// </summary>
public sealed class TransparentDesktopWindow : TransparentWindow, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Transparent;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public TransparentDesktopWindow(IServiceProvider context)
        : base(context, "Transparent window", "AppKit handles the native window drag and resize frame; Xui draws the rounded content surface.")
    {
        Title = "Transparent Window";
    }
}
