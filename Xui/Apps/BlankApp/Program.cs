#if !BROWSER
using Xui.Core.Abstract;
using Xui.Core.Actual;

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
#endif

        return new App().Run();;
    }

    public override void Start()
    {
        var window = new MainWindow();
        window.Title = "Xui BlankApp";
        window.Show();
    }
}

#else

using System;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

Console.WriteLine("Hello, Browser!");

if (args.Length == 1 && args[0] == "start")
    StopwatchSample.Start();

while(true)
{
    StopwatchSample.Render();
    await Task.Delay(1000);
}

partial class StopwatchSample
{
    private static Stopwatch stopwatch = new();

    public static void Start() => stopwatch.Start();
    public static void Render() => SetInnerText("#time", stopwatch.Elapsed.ToString(@"mm\:ss"));
    
    [JSImport("dom.setInnerText", "main.js")]
    internal static partial void SetInnerText(string selector, string content);

    [JSExport]
    internal static bool Toggle()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            return false;
        }
        else
        {
            stopwatch.Start();
            return true;
        }
    }

    [JSExport]
    internal static void Reset()
    {
        if (stopwatch.IsRunning)
            stopwatch.Restart();
        else
            stopwatch.Reset();

        Render();
    }

    [JSExport]
    internal static bool IsRunning() => stopwatch.IsRunning;
}
#endif