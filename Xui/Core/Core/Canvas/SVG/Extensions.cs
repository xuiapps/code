namespace Xui.Core.Canvas.SVG;

public static class Extensions
{
    public static PathDataBuilder PathData(this IPathDrawingContext @this) => new PathDataBuilder(@this);
}
