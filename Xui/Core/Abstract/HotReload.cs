using Xui.Core.Actual;

[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(Xui.Core.Abstract.HotReload))]

namespace Xui.Core.Abstract;

public static class HotReload
{
    public static void ClearCache(Type[]? updatedTypes)
    {
    }

    public static void UpdateApplication(Type[]? updatedTypes) =>
        Runtime.Current?.MainDispatcher?.Post(() =>
            MainThreadUpdateApplication(updatedTypes));
    
    public static void MainThreadUpdateApplication(Type[]? updatedTypes)
    {
        foreach(var window in Window.OpenWindows)
        {
            window.Invalidate();
        }
    }
}