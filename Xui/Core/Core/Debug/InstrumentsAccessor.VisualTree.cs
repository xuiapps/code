using Xui.Core.UI;

namespace Xui.Core.Debug;

public static class InstrumentsAccessorExtensions
{
    public static void DumpVisualTree(this InstrumentsAccessor instruments, View root, LevelOfDetail lod = LevelOfDetail.Info)
    {
        if (instruments.Sink == null || !instruments.Sink.IsEnabled(Scope.ViewLifecycle, lod))
            return;

        DumpView(instruments, root, 0, lod);
    }

    private static void DumpView(InstrumentsAccessor instruments, View view, int depth, LevelOfDetail lod)
    {
        var frame = view.Frame;
        instruments.Log(Scope.ViewLifecycle, lod,
            $"{new string(' ', depth * 2)}{view.GetType().Name} Frame({frame.X:F1}, {frame.Y:F1}, {frame.Width:F1}, {frame.Height:F1})");

        for (int i = 0; i < view.Count; i++)
        {
            DumpView(instruments, view[i], depth + 1, lod);
        }
    }
}
