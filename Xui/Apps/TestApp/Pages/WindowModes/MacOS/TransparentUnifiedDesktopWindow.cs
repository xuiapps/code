using Xui.Core.Abstract;
using Xui.Core.Math2D;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// A transparent desktop window with a unified native toolbar.
/// </summary>
public sealed class TransparentUnifiedDesktopWindow : TransparentWindow, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Transparent;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public TransparentUnifiedDesktopWindow(IServiceProvider context)
        : base(context, "Transparent unified", "A rounded Xui frame with a unified native toolbar.")
    {
        Title = "Transparent Unified";
    }
}
