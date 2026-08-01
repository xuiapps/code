using Xui.Core.Abstract;
using Xui.Core.Math2D;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// A transparent desktop window with a compact unified native toolbar.
/// </summary>
public sealed class TransparentUnifiedCompactDesktopWindow : TransparentWindow, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Medium;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Transparent;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public TransparentUnifiedCompactDesktopWindow(IServiceProvider context)
        : base(context, "Transparent unified compact", "A rounded Xui frame with a compact unified native toolbar.")
    {
        Title = "Transparent Unified Compact";
    }
}
