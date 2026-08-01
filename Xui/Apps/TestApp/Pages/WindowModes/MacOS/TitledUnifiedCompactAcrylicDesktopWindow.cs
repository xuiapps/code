using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>
/// A titled compact-unified window with an Acrylic background.
/// </summary>
public sealed class TitledUnifiedCompactAcrylicDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Medium;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Acrylic;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public TitledUnifiedCompactAcrylicDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Titled Unified Compact Acrylic";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Titled unified compact Acrylic",
            "The native title stays visible above a compact unified toolbar and Acrylic client background.");
    }
}
