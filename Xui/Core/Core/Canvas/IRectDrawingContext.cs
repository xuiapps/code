using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Provides convenience methods for drawing filled and stroked rectangles,
/// similar to the <c>fillRect()</c> and <c>strokeRect()</c> functions in the HTML5 Canvas API.
/// </summary>
public interface IRectDrawingContext
{
    /// <summary>
    /// Draws the outline of the specified rectangle using the current stroke style.
    /// </summary>
    /// <param name="rect">The rectangle to stroke.</param>
    void StrokeRect(Rect rect);

    /// <summary>
    /// Fills the specified rectangle using the current fill style.
    /// </summary>
    /// <param name="rect">The rectangle to fill.</param>
    void FillRect(Rect rect);
}
