namespace Xui.Apps.BlankApp;

public class App : Xui.Core.Abstract.Application
{
    static int Main(string[] argv)
    {
#if MACOS && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Middleware.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.MacOS.Actual.MacOSPlatform.Instance);
#elif WINDOWS && EMULATOR
        Xui.Core.Actual.Runtime.Current = new Xui.Emulator.Emulator.Actual.EmulatorPlatform(
            Xui.Runtime.Windows.Actual.Win32Platform.Instance);
#elif IOS
        Xui.Core.Actual.Runtime.Current = Xui.Runtime.IOS.Actual.IOSPlatform.Instance;
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
        var window = new MainWindow();
        window.Title = "Xui BlankApp";
        window.Show();
    }
}
