using System;
using Xui.Core.Math2D;
using Xui.Core.Middleware;
using Xui.Core.Abstract;
using AppKit = Xui.Runtime.MacOS.AppKit;
using Foundation = Xui.Runtime.MacOS.Foundation;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private static Style GetStyle(Xui.Core.Abstract.IWindow @abstract)
    {
        if (@abstract.GetMacOSWindowStyle() is not { } style)
            return GetCrossPlatformStyle(@abstract.GetDesktopStyle());

        return (style.Title, style.TitleHeight, style.Background) switch
        {
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Default, MacOSWindowBackground.Default) => TitledStyle.Instance,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Medium, MacOSWindowBackground.Default) => TitledUnifiedCompactStyle.Instance,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Large, MacOSWindowBackground.Default) => TitledUnifiedStyle.Instance,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Default, MacOSWindowBackground.Default) => UntitledStyle.Instance,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Medium, MacOSWindowBackground.Default) => UntitledUnifiedCompactStyle.Instance,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Large, MacOSWindowBackground.Default) => UntitledUnifiedStyle.Instance,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Default, MacOSWindowBackground.Transparent) => TransparentStyle.Instance,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Medium, MacOSWindowBackground.Transparent) => TransparentUnifiedCompactStyle.Instance,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Large, MacOSWindowBackground.Transparent) => TransparentUnifiedStyle.Instance,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Default, MacOSWindowBackground.Transparent) => TransparentStyle.Instance,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Medium, MacOSWindowBackground.Transparent) => TransparentUnifiedCompactStyle.Instance,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Large, MacOSWindowBackground.Transparent) => TransparentUnifiedStyle.Instance,
            (_, _, MacOSWindowBackground.Acrylic) => AcrylicStyle.Get(style.Title, style.TitleHeight),
            (_, _, MacOSWindowBackground.Glass) => GlassStyle.Get(style.Title, style.TitleHeight),
            (_, _, MacOSWindowBackground.Borderless) => BorderlessStyle.Get(style.TitleHeight),
            _ => throw new ArgumentOutOfRangeException(nameof(style), "Unsupported macOS window style."),
        };
    }

    private static Style GetCrossPlatformStyle(Xui.Core.Abstract.IWindow.IDesktopStyle? style)
    {
        if (style is null)
            return TitledStyle.Instance;

        return style.Backdrop switch
        {
            // The previous Chromeless implementation used a full-size,
            // transparent title area with AppKit's unified toolbar.
            WindowBackdrop.Chromeless => BorderlessStyle.Get(MacOSWindowTitleHeight.Large),

            // macOS has no Mica material; both legacy backdrops used the
            // visual-effect treatment now represented by Acrylic.
            WindowBackdrop.Mica or WindowBackdrop.Acrylic =>
                AcrylicStyle.Get(MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Large),

            // The old extended client area hid the title and made the full
            // content view available to Xui without adding a toolbar.
            _ when style.ClientArea == WindowClientArea.Extended => UntitledStyle.Instance,

            _ => TitledStyle.Instance,
        };
    }

    private abstract class Style
    {
        public abstract void ConfigureInitialWindow(
            Xui.Core.Abstract.IWindow @abstract,
            ref NSWindowStyleMask mask,
            ref Rect rect);

        public abstract void ConfigureContentView(MacOSWindow window, Foundation.NSRect initialContentFrame);

        public abstract Rect GetLayoutArea(MacOSWindow window, Rect rootArea);

        public abstract bool ShouldSoftwareTitleBarZoom(MacOSWindow window);

        public virtual void OnEventDispatched(MacOSWindow window)
        {
        }

        public virtual void OnWindowDidResize(MacOSWindow window)
        {
        }
    }

}
