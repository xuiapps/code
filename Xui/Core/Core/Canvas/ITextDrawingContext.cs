using System;
using Xui.Core.Math2D;
using Xui.Core.Memory;

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

    /// <summary>
    /// Draws filled text at the specified position without allocating a string.
    /// Use with <c>stackalloc</c> and <see cref="ISpanFormattable.TryFormat"/> for zero-allocation rendering.
    /// </summary>
    /// <param name="text">The text characters to render.</param>
    /// <param name="pos">The position at which to start rendering the text.</param>
    void FillText(ReadOnlySpan<char> text, Point pos) => FillText(new string(text), pos);

    /// <summary>
    /// Draws filled text from an interpolated string without heap-allocating the string.
    /// The compiler automatically prefers this overload for interpolated string arguments.
    /// </summary>
    /// <param name="handler">The interpolated string handler that formats into a stack buffer.</param>
    /// <param name="pos">The position at which to start rendering the text.</param>
    void FillText(FillTextInterpolatedStringHandler handler, Point pos)
    {
        FillText(handler.AsSpan(), pos);
        handler.Dispose();
    }
}
