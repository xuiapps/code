using Xui.Apps.TestApp.Pages;
using Xui.Core.Abstract;

namespace Xui.Apps.TestApp;

public class MainWindow : Window
{
    public MainWindow(IServiceProvider context) : base(context)
    {
        this.Title = "Xui TestApp";
        var instruments = this.RootView.RenderSurfaceInstruments as LayoutFrameInstruments
            ?? new LayoutFrameInstruments();
        var testSettings = context.GetService(typeof(ITestSettings)) as ITestSettings;
        this.Content = new TestAppShell(
            new SdkNavigation(),
            instruments,
            testSettings?.IsDebugOverlayEnabled ?? true);
    }
}
