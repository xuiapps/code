using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// A standard desktop window with a compact unified native toolbar.
/// </summary>
public sealed class TitledUnifiedCompactDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Medium;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public TitledUnifiedCompactDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Titled Unified Compact";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Titled unified compact",
            "The native title remains visible above Xui's normal client area.");
    }
}
