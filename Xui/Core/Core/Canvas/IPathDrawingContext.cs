using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Defines methods for constructing and rendering 2D paths,
/// following the HTML5 Canvas path API model.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathDrawingContext
{
    /// <summary>
    /// Begins a new path by resetting the current path list.
    /// </summary>
    void BeginPath();

    /// <summary>
    /// Moves the current point to the specified location without drawing a line.
    /// </summary>
    /// <param name="to">The destination point.</param>
    void MoveTo(Point to);

    /// <summary>
    /// Connects the current point to the specified point with a straight line.
    /// </summary>
    /// <param name="to">The endpoint of the line.</param>
    void LineTo(Point to);

    /// <summary>
    /// Closes the current sub-path by drawing a straight line back to the start point.
    /// </summary>
    void ClosePath();

    /// <summary>
    /// Draws a quadratic Bézier curve from the current point to the specified point,
    /// using the given control point.
    /// </summary>
    /// <param name="cp1">Control point.</param>
    /// <param name="to">End point.</param>
    void CurveTo(Point cp1, Point to);

    /// <summary>
    /// Draws a cubic Bézier curve from the current point to the specified point,
    /// using two control points.
    /// </summary>
    /// <param name="cp1">First control point.</param>
    /// <param name="cp2">Second control point.</param>
    /// <param name="to">End point.</param>
    void CurveTo(Point cp1, Point cp2, Point to);

    /// <summary>
    /// Adds an arc to the path centered at the specified point.
    /// </summary>
    /// <param name="center">Center of the arc.</param>
    /// <param name="radius">Radius of the arc.</param>
    /// <param name="startAngle">Start angle in radians.</param>
    /// <param name="endAngle">End angle in radians.</param>
    /// <param name="winding">Direction in which the arc is drawn.</param>
    void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise);

    /// <summary>
    /// Adds an arc to the path, connecting two tangents defined by control points.
    /// </summary>
    /// <param name="cp1">First control point.</param>
    /// <param name="cp2">Second control point.</param>
    /// <param name="radius">Arc radius.</param>
    void ArcTo(Point cp1, Point cp2, nfloat radius);

    /// <summary>
    /// Adds an elliptical arc to the path.
    /// </summary>
    /// <param name="center">Center of the ellipse.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="rotation">Rotation of the ellipse, in radians.</param>
    /// <param name="startAngle">Start angle in radians.</param>
    /// <param name="endAngle">End angle in radians.</param>
    /// <param name="winding">Direction in which the arc is drawn.</param>
    void Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise);

    /// <summary>
    /// Adds a rectangle path to the current path.
    /// </summary>
    /// <param name="rect">The rectangle to add.</param>
    void Rect(Rect rect);

    /// <summary>
    /// Adds a rounded rectangle path with a uniform radius.
    /// </summary>
    /// <param name="rect">The rectangle to round.</param>
    /// <param name="radius">Corner radius.</param>
    void RoundRect(Rect rect, nfloat radius);

    /// <summary>
    /// Adds a rounded rectangle path with per-corner radii.
    /// </summary>
    /// <param name="rect">The rectangle to round.</param>
    /// <param name="radius">Corner radius object defining each corner.</param>
    void RoundRect(Rect rect, CornerRadius radius);

    /// <summary>
    /// Fills the current path using the specified fill rule.
    /// </summary>
    /// <param name="rule">The fill rule to use (NonZero or EvenOdd).</param>
    void Fill(FillRule rule = FillRule.NonZero);

    /// <summary>
    /// Strokes the current path using the current stroke style.
    /// </summary>
    void Stroke();

    /// <summary>
    /// Sets the current clipping region to the current path.
    /// Subsequent drawing operations are clipped to this region.
    /// </summary>
    void Clip();
}
