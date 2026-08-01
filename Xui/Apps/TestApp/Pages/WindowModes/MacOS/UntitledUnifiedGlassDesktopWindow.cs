using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>An untitled unified window backed by the dynamic macOS glass material.</summary>
public sealed class UntitledUnifiedGlassDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Glass;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public UntitledUnifiedGlassDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled Unified Glass";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Untitled unified Glass",
            "A dynamic macOS glass background extending through the unified title region.");
    }
}
