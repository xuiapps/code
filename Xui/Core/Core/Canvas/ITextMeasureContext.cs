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

    /// <summary>
    /// Returns the character index (caret position) in <paramref name="text"/>
    /// closest to the given <paramref name="x"/> coordinate, using the current font.
    /// The result ranges from 0 (before the first character) to text.Length
    /// (after the last character). The position snaps to whichever side of
    /// a character's midpoint <paramref name="x"/> falls on.
    /// </summary>
    /// <param name="text">The text to hit-test against.</param>
    /// <param name="x">The horizontal offset to test, relative to the text origin.</param>
    /// <returns>The nearest caret position (0 to text.Length).</returns>
    /// <inheritdoc cref="HitTestTextPosition(ReadOnlySpan{char}, nfloat)"/>
    int HitTestTextPosition(string text, nfloat x) => HitTestTextPosition(text.AsSpan(), x);

    /// <inheritdoc cref="HitTestTextPosition(string, nfloat)"/>
    int HitTestTextPosition(ReadOnlySpan<char> text, nfloat x)
    {
        var len = text.Length;
        if (len == 0)
            return 0;

        var fullWidth = MeasureText(text).Size.Width;
        if (x >= fullWidth)
            return len;
        if (x <= 0)
            return 0;

        // Binary search — character widths are monotonically increasing.
        // Use x/fullWidth ratio as initial hint to narrow the range.
        var hint = (int)(x / fullWidth * len);
        var lo = 0;
        var hi = len;

        if (hint > 1) lo = hint - 1;
        if (hint + 1 < len) hi = hint + 1;

        // Expand until the range brackets x.
        while (lo > 0)
        {
            var w = MeasureText(text[..lo]).Size.Width;
            if (w <= x) break;
            lo = Math.Max(0, lo - (lo / 2 == 0 ? 1 : lo / 2));
        }
        while (hi < len)
        {
            var w = MeasureText(text[..hi]).Size.Width;
            if (w >= x) break;
            hi = Math.Min(len, hi + ((len - hi) / 2 == 0 ? 1 : (len - hi) / 2));
        }

        // Standard binary search within [lo, hi].
        while (lo < hi)
        {
            var mid = (lo + hi) / 2;
            var w = MeasureText(text[..(mid + 1)]).Size.Width;
            if (w < x)
                lo = mid + 1;
            else
                hi = mid;
        }

        // Snap to nearest edge: before or after character lo.
        var wLeft = lo > 0 ? MeasureText(text[..lo]).Size.Width : 0;
        var wRight = MeasureText(text[..(lo + 1)]).Size.Width;
        return x < (wLeft + wRight) / 2 ? lo : lo + 1;
    }
}
