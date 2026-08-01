using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>An untitled compact-unified window backed by the dynamic macOS glass material.</summary>
public sealed class UntitledUnifiedCompactGlassDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Medium;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Glass;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public UntitledUnifiedCompactGlassDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled Unified Compact Glass";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Untitled unified compact Glass",
            "A dynamic macOS glass background extending through the compact unified title region.");
    }
}
