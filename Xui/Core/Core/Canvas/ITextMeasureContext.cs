using System;
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Provides functionality for measuring text and setting font properties.
/// Mirrors the measurement behavior of the HTML5 Canvas 2D context,
/// but returns a full <see cref="TextMetrics"/> structure for advanced layout.
/// </summary>
public interface ITextMeasureContext
{
    /// <summary>
    /// Measures the layout and bounding box metrics of the specified text string
    /// using the current font.
    /// </summary>
    /// <param name="text">The text string to measure.</param>
    /// <returns>
    /// A <see cref="TextMetrics"/> structure containing both string-specific
    /// and font-wide metrics.
    /// </returns>
    TextMetrics MeasureText(string text);

    /// <summary>
    /// Measures the layout and bounding box metrics of the specified text
    /// without allocating a string.
    /// </summary>
    /// <param name="text">The text characters to measure.</param>
    /// <returns>
    /// A <see cref="TextMetrics"/> structure containing both string-specific
    /// and font-wide metrics.
    /// </returns>
    TextMetrics MeasureText(ReadOnlySpan<char> text) => MeasureText(new string(text));

    /// <summary>
    /// Sets the font used for all subsequent text drawing and measurement operations.
    /// </summary>
    /// <param name="font">The font definition to apply.</param>
    void SetFont(Font font);
}
