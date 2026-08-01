using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.TestApp.Pages.WindowModes.MacOS;

/// <summary>A desktop window with a hidden title and compact unified native toolbar.</summary>
public sealed class UntitledUnifiedCompactDesktopWindow : Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Medium;
    Size? IWindow.IDesktopStyle.StartupSize => new(560, 360);

    public UntitledUnifiedCompactDesktopWindow(IServiceProvider context) : base(context)
    {
        Title = "Untitled Unified Compact";
        Content = UntitledUnifiedDesktopWindow.CreateContent(
            "Untitled unified compact",
            "A visible compact unified native toolbar expands the title region.");
    }
}
