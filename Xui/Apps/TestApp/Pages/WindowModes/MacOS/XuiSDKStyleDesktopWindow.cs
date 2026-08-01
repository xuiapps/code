using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// An untitled unified window with an Acrylic background.
/// </summary>
public sealed class XuiSDKStyleDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    private static readonly NFloat HeaderHeight = 52;

    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Acrylic;
    Size? IWindow.IDesktopStyle.StartupSize => new(700, 420);

    public XuiSDKStyleDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "XuiSDK Title Area";
        Content = new Border
        {
            BackgroundColor = new Color(0xF7F7F980),
            Content = new VerticalStack
            {
                Content =
                [
                    new Border
                    {
                        MinimumHeight = HeaderHeight,
                        BackgroundColor = new Color(0x8A05FFFF),
                        Content = new Label
                        {
                            Text = "XuiSDK-style extended title area",
                            Font = new Font(15, "Inter", FontWeight.SemiBold),
                            TextColor = Colors.White,
                            VerticalAlignment = VerticalAlignment.Middle,
                            Margin = (0, 0, 0, 86),
                        },
                    },
                    new VerticalStack
                    {
                        Margin = 32,
                        Content =
                        [
                            new Label
                            {
                                Text = "Client area extends into the title area",
                                Font = new Font(18, "Inter", FontWeight.SemiBold),
                                Margin = (0, 0, 8, 0),
                            },
                            new Label
                            {
                                Text = "The header is draggable and the macOS traffic lights sit within the Xui title region.",
                                FontFamily = "Inter",
                                TextColor = new Color(0x606068FF),
                            },
                        ],
                    },
                ],
            },
        };
    }
}
