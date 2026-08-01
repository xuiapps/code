using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>An untitled window backed by the dynamic macOS glass material.</summary>
public sealed class UntitledGlassDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Glass;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public UntitledGlassDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled Glass";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Untitled Glass",
            "A dynamic macOS glass background extending through the title region.");
    }
}
