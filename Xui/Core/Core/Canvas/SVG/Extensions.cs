namespace Xui.Core.Canvas.SVG;

/// <summary>
/// Provides SVG-related extension methods for the <see cref="IPathDrawingContext"/> interface.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Creates a <see cref="PathDataBuilder"/> for the given path drawing context.
    /// 
    /// This enables fluent parsing or construction of SVG path commands targeting the current canvas context.
    /// </summary>
    /// <param name="this">The path drawing context (e.g., a canvas or drawing surface).</param>
    /// <returns>A <see cref="PathDataBuilder"/> instance for building or parsing SVG paths.</returns>
    public static PathDataBuilder PathData(this IPathDrawingContext @this) => new PathDataBuilder(@this);
}
