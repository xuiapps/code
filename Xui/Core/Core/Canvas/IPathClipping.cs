namespace Xui.Core.Canvas;

/// <summary>
/// Defines methods for clipping using a path,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathClipping
{
    /// <summary>
    /// Sets the current clipping region to the current path.
    /// Subsequent drawing operations are clipped to this region.
    /// </summary>
    void Clip();
}
