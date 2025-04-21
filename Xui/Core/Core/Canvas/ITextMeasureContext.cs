using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Provides functionality for measuring text and setting font properties.
/// Mirrors the measurement behavior of the HTML5 Canvas 2D context.
/// </summary>
public interface ITextMeasureContext
{
    /// <summary>
    /// Measures the width and height of the specified text string using the current font.
    /// </summary>
    /// <param name="text">The text string to measure.</param>
    /// <returns>
    /// A <see cref="Vector"/> representing the size of the rendered text.
    /// Typically, X is width and Y is line height or baseline ascent.
    /// </returns>
    Vector MeasureText(string text);

    /// <summary>
    /// Sets the font used for all subsequent text drawing and measurement operations.
    /// </summary>
    /// <param name="font">The font definition to apply.</param>
    void SetFont(Font font);
}
