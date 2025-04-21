using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Defines methods for drawing text onto the canvas, including alignment and baseline options.
/// Mirrors the HTML5 Canvas 2D text API for <c>fillText()</c>.
/// Also provides access to text measurement via <see cref="ITextMeasureContext"/>.
/// </summary>
public interface ITextDrawingContext : ITextMeasureContext
{
    /// <summary>
    /// Sets the horizontal alignment of the text relative to the given position.
    /// </summary>
    TextAlign TextAlign { set; }

    /// <summary>
    /// Sets the vertical alignment of the text relative to the given baseline.
    /// </summary>
    TextBaseline TextBaseline { set; }

    /// <summary>
    /// Draws filled text at the specified position using the current fill style and font settings.
    /// </summary>
    /// <param name="text">The text string to render.</param>
    /// <param name="pos">The position at which to start rendering the text.</param>
    void FillText(string text, Point pos);
}
