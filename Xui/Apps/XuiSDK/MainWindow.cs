using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;

namespace Xui.Apps.XuiSDK;

public class MainWindow : Xui.Core.Abstract.Window, IWindow.IDesktopStyle, IMacOSWindowStyle
{
    public WindowRenderingMetrics PerformanceMetrics { get; } = new();

    WindowBackdrop IWindow.IDesktopStyle.Backdrop =>
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? WindowBackdrop.Default : WindowBackdrop.Mica;
    Size? IWindow.IDesktopStyle.StartupSize => new Size(900, 600);
    WindowClientArea IWindow.IDesktopStyle.ClientArea => WindowClientArea.Extended;

    MacOSWindowTitle IMacOSWindowStyle.Title => MacOSWindowTitle.Hidden;
    MacOSWindowTitleHeight IMacOSWindowStyle.TitleHeight => MacOSWindowTitleHeight.Large;
    MacOSWindowBackground IMacOSWindowStyle.Background => MacOSWindowBackground.Acrylic;

    public MainWindow(IServiceProvider context) : base(context)
    {
        Content = new NavigationShell(PerformanceMetrics);
    }

    public override void Render(ref RenderEventRef renderEventRef)
    {
        bool animated = (RootView.Flags & (View.ViewFlags.Animated | View.ViewFlags.DescendantAnimated)) != 0;
        PerformanceMetrics.BeginFrame(animated);
        base.Render(ref renderEventRef);
        PerformanceMetrics.EndFrame();
    }

}
