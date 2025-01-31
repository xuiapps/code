using Xui.Core.Abstract;
using Xui.Core.Actual;

namespace FirstApp;

public class App : Application
{
    static int Main(string[] argv)
    {
// #if MACOS && EMULATOR
//         Platform.Current = new Xui.Emulator.Actual.EmulatorPlatform(
//             Xui.MacOS.Actual.MacOSPlatform.Instance);
// #elif WINDOWS && EMULATOR
//         Platform.Current = new Xui.Emulator.Actual.EmulatorPlatform(
//             Xui.Windows.Actual.Win32Platform.Instance);
// #elif IOS
//         Platform.Current = Xui.IOS.Actual.IOSPlatform.Instance;
// #elif MACOS
//         Platform.Current = Xui.MacOS.Actual.MacOSPlatform.Instance;
// #elif WINDOWS
//         Platform.Current = Xui.Windows.Actual.Win32Platform.Instance;
// #endif

        return new App().Run();;
    }

    public override void Start()
    {
        var window = new MainWindow();
        window.Title = "APP# FirstApp";
        window.Show();
    }
}
