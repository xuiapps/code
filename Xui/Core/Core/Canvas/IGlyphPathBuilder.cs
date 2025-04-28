using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// A lightweight interface for constructing TrueType glyph paths
/// using move, line, curve, and close commands.
/// </summary>
/// <remarks>
/// Glyphs in TrueType fonts use quadratic Bézier curves with optional
/// off-curve control points. This interface is intended for glyph outlines
/// and font rendering engines.
/// </remarks>
public interface IGlyphPathBuilder
{
    /// <summary>
    /// Begins a new sub-path at the specified point.
    /// </summary>
    void MoveTo(Point to);

    /// <summary>
    /// Draws a straight line to the specified point.
    /// </summary>
    void LineTo(Point to);

    /// <summary>
    /// Draws a quadratic Bézier curve using a control point and end point.
    /// </summary>
    void CurveTo(Point control, Point to);

    /// <summary>
    /// Closes the current path contour.
    /// </summary>
    void ClosePath();
}
