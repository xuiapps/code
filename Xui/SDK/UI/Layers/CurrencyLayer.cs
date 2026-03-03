// File: Xui/SDK/UI/Layers/CurrencyLayer.cs
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layers;

namespace Xui.SDK.UI.Layers;

/// <summary>
/// Leaf layer for currency display and editing.
/// Unfocused: renders a formatted currency string (e.g. <c>$1,234.56</c>), with an
///   overflow mask (e.g. <c>$#,###.##</c>) when the available width is too narrow.
/// Focused + SelectAll: shows the formatted text covered by a selection highlight.
/// Focused + editing: shows the formatted text with a blinking caret at the right end.
/// </summary>
public struct CurrencyLayer : ILeaf
{
    public decimal Value;
    public string CurrencySymbol;
    public bool SymbolPrefix;
    public string Format;

    public bool IsFocused;

    /// <summary>
    /// Toggled by the owning view's animation loop to produce caret blinking.
    /// Has no effect when <see cref="SelectAll"/> is true (selection doesn't blink).
    /// </summary>
    public bool CaretVisible;

    /// <summary>
    /// When true and focused, the entire text area is rendered with a selection
    /// highlight (blue rect / white text), matching "select all on focus" behavior.
    /// </summary>
    public bool SelectAll;

    // --- Typography ---

    public string[]? FontFamily;
    public nfloat FontSize;

    // --- Colors ---

    public Color TextColor;
    public Color SelectionBackgroundColor;
    public Color SelectedTextColor;

    // --- Layout ---

    public nfloat Padding;

    // --- Cached state (set during Measure) ---

    private bool _overflow;

    // ---

    private Font GetFont() => new Font { FontFamily = FontFamily, FontSize = FontSize };

    private string FormatFull()
    {
        var sym = CurrencySymbol ?? string.Empty;
        var num = Value.ToString(Format ?? "N2");
        return SymbolPrefix ? sym + num : num + sym;
    }

    private string FormatMask()
    {
        var full = FormatFull();
        var chars = full.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            if (char.IsAsciiDigit(chars[i])) chars[i] = '#';
        return new string(chars);
    }

    public LayoutGuide Update(LayoutGuide guide)
    {
        if (guide.IsMeasure)
        {
            var ctx = guide.MeasureContext!;
            ctx.SetFont(GetFont());
            var full = FormatFull();
            var measured = ctx.MeasureText(full).Size;
            var width = measured.Width + Padding * 2;
            var height = measured.Height + Padding * 2;

            _overflow = guide.AvailableSize.Width > 0 && width > guide.AvailableSize.Width;

            var desiredWidth = _overflow
                ? (guide.AvailableSize.Width > 0 ? guide.AvailableSize.Width : width)
                : width;

            guide.DesiredSize = new Size(desiredWidth, height);
        }

        if (guide.IsRender)
        {
            var ctx = guide.RenderContext!;
            var r = guide.ArrangedRect;

            ctx.SetFont(GetFont());
            ctx.TextBaseline = TextBaseline.Top;
            ctx.TextAlign = TextAlign.Left;

            var text = _overflow ? FormatMask() : FormatFull();
            nfloat textX = r.X + Padding;
            nfloat textY = r.Y + Padding;

            if (IsFocused && SelectAll)
            {
                // Full-text selection highlight
                var textW = ctx.MeasureText(text).Size.Width;
                ctx.SetFill(SelectionBackgroundColor);
                ctx.FillRect(new Rect(textX, r.Y + 2, textW, r.Height - 4));

                ctx.SetFill(SelectedTextColor);
                ctx.FillText(text, new Point(textX, textY));
            }
            else
            {
                ctx.SetFill(TextColor);
                ctx.FillText(text, new Point(textX, textY));

                // Blinking caret at the right end of the text when editing
                if (IsFocused && CaretVisible)
                {
                    var textW = ctx.MeasureText(text).Size.Width;
                    ctx.SetFill(TextColor);
                    ctx.FillRect(new Rect(textX + textW, r.Y + Padding, 1, r.Height - Padding * 2));
                }
            }
        }

        return guide;
    }
}
