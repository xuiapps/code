using Xui.Core.UI;

namespace Xui.Core.Debug;

/// <summary>Extension methods on <see cref="InstrumentsAccessor"/> for visual tree diagnostics.</summary>
public static class InstrumentsAccessorExtensions
{
    /// <summary>Logs the full visual tree rooted at <paramref name="root"/> at the given level of detail.</summary>
    /// <param name="instruments">The accessor to log through.</param>
    /// <param name="root">The root view to dump.</param>
    /// <param name="lod">The level of detail threshold.</param>
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
