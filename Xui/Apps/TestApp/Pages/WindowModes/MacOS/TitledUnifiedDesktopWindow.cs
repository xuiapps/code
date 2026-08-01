using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// A standard desktop window with a unified native toolbar.
/// </summary>
public sealed class TitledUnifiedDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public TitledUnifiedDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Titled Unified";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Titled unified",
            "The native title remains visible above Xui's normal client area.");
    }
}
