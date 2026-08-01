using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>An untitled unified window with an Acrylic background.</summary>
public sealed class UntitledUnifiedAcrylicDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Acrylic;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public UntitledUnifiedAcrylicDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled Unified Acrylic";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Untitled unified Acrylic",
            "An Acrylic background extending through the unified title region.");
    }
}
