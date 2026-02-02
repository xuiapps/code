using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;
using Xui.Core.Canvas;

namespace Xui.Apps.BlankApp;

public class App : Xui.Core.Abstract.Application
{
    public static int Main(string[] argv)
    {
#if MACOS && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.MacOS.Actual.MacOSPlatform.Instance);
#elif WINDOWS && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.Windows.Actual.Win32Platform.Instance);
#elif BROWSER && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.Browser.Actual.BrowserPlatform.Instance);
#elif IOS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.IOS.Actual.IOSPlatform.Instance;
#elif ANDROID
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.Android.Actual.AndroidPlatform.Instance;
#elif MACOS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.MacOS.Actual.MacOSPlatform.Instance;
#elif WINDOWS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.Windows.Actual.Win32Platform.Instance;
#elif BROWSER
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.Browser.Actual.BrowserPlatform.Instance;
#endif

        return new App().Run();
    }

    public override void Start()
    {
        var window = new MainWindow
        {
            Title = "Xui ClockApp",
        };

        window.Show();
    }
}
