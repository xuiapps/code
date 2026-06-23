using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Runtime.Test;

namespace Xui.Tests.Integration.TestApp;

public static class IntegrationRuntimeVariants
{
    public static IEnumerable<object[]> All
    {
        get
        {
            yield return [TestRuntimeVariant.Desktop];
            yield return [TestRuntimeVariant.IPhoneEmulator];
        }
    }

    public static TestSinglePageApp<TApplication, TWindow> CreateApp<TApplication, TWindow>(
        Size desktopSize,
        TestRuntimeVariant runtimeVariant,
        string callerPath,
        string testName)
        where TApplication : Application
        where TWindow : Window
    {
        var size = runtimeVariant == TestRuntimeVariant.IPhoneEmulator
            ? new Size(NFloat.Max(desktopSize.Width, 460), NFloat.Max(desktopSize.Height, 980))
            : desktopSize;

        var snapshotSet = runtimeVariant == TestRuntimeVariant.IPhoneEmulator
            ? "Scenarios.Emulator.iPhone15Pro"
            : null;

        return new TestSinglePageApp<TApplication, TWindow>(
            size,
            runtimeVariant: runtimeVariant,
            snapshotSet: snapshotSet,
            callerPath: callerPath,
            testName: testName);
    }
}
