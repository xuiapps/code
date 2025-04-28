using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Defines methods for constructing and rendering 2D paths,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathDrawingContext : IPathBuilder, IPathDrawing, IPathClipping
{
}
