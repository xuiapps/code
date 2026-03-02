// File: Xui/Core/UI/Layers/TextInputLayer.cs
using Xui.Core.Canvas;
using Xui.Core.Math1D;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Core.UI.Layers;

/// <summary>
/// Leaf layer that renders a single line of editable text with optional selection
/// highlight and a blinking insertion caret. All state is managed by the owning
/// <see cref="View"/> (typically <see cref="Xui.Core.UI.TextBox"/>); this layer
/// is responsible for rendering only.
/// </summary>
public struct TextInputLayer : ILeaf
{
    // --- Text state (kept in sync by the owning View) ---

    /// <summary>Current text content to render.</summary>
    public string? Text;

    /// <summary>
    /// Selection range as a half-open interval [Start, End).
    /// When Start == End the interval represents the caret position.
    /// </summary>
    public Interval<uint>.ClosedOpen Selection;

    /// <summary>When true, characters are replaced with bullet (•) during rendering.</summary>
    public bool IsPassword;

    // --- Focus/caret state (set by the owning View) ---

    /// <summary>Whether the owning view has keyboard focus (shows selection highlight).</summary>
    public bool IsFocused;

    /// <summary>Toggled by the owning View's animation loop to produce caret blinking.</summary>
    public bool CaretVisible;

    // --- Typography ---

    public string[]? FontFamily;
    public nfloat FontSize;
    public FontStyle FontStyle;
    public FontWeight FontWeight;
    public FontStretch FontStretch;

    // --- Colors ---

    public Color TextColor;
    public Color SelectedColor;
    public Color SelectionBackgroundColor;

    // --- Layout ---

    /// <summary>Horizontal padding in pixels on each side of the text.</summary>
    public nfloat Padding;

    private Font GetFont() => new Font
    {
        FontFamily = FontFamily,
        FontSize = FontSize,
        FontStyle = FontStyle,
        FontWeight = FontWeight,
        FontStretch = FontStretch,
    };

    private string GetDisplayText()
    {
        var text = Text ?? string.Empty;
        return IsPassword ? new string('\u2022', text.Length) : text;
    }

    public LayoutGuide Update(LayoutGuide guide)
    {
        if (guide.IsMeasure)
        {
            var context = guide.MeasureContext!;
            context.SetFont(GetFont());
            var displayText = GetDisplayText();
            var metrics = context.MeasureText(displayText.Length > 0 ? displayText : "X");
            var height = metrics.Size.Height + Padding * 2;
            // Fill available width; enforce a minimum of 100 px so an empty TextBox is usable
            var width = guide.AvailableSize.Width > 100 ? guide.AvailableSize.Width : (nfloat)100;
            guide.DesiredSize = new Size(width, height);
        }

        if (guide.IsRender)
        {
            var context = guide.RenderContext!;
            var frame = guide.ArrangedRect;

            context.SetFont(GetFont());
            context.TextBaseline = TextBaseline.Top;
            context.TextAlign = TextAlign.Left;

            var displayText = GetDisplayText();
            var sel = Selection;
            var textX = frame.X + Padding;
            var textY = frame.Y + Padding;

            if (!sel.IsEmpty && IsFocused)
            {
                var selStart = (int)sel.Start;
                var selEnd = (int)sel.End;

                // Measure widths needed to position the selection highlight.
                // The formula accounts for sub-pixel kerning: selStartX = leftAndSelWidth - selWidth.
                var leftWidth = selStart > 0
                    ? context.MeasureText(displayText[..selStart]).Size.Width
                    : (nfloat)0;
                var selWidth = context.MeasureText(displayText[selStart..selEnd]).Size.Width;
                var leftAndSelWidth = context.MeasureText(displayText[..selEnd]).Size.Width;
                var selStartX = leftAndSelWidth - selWidth;

                var allWidth = context.MeasureText(displayText).Size.Width;
                var rightWidth = selEnd < displayText.Length
                    ? context.MeasureText(displayText[selEnd..]).Size.Width
                    : (nfloat)0;

                // Selection background
                context.SetFill(SelectionBackgroundColor);
                context.FillRect(new Rect(
                    textX + selStartX,
                    frame.Y + Padding,
                    selWidth,
                    frame.Height - Padding * 2));

                // Text before selection
                if (selStart > 0)
                {
                    context.SetFill(TextColor);
                    context.FillText(displayText[..selStart], new Point(textX, textY));
                }

                // Selected text
                context.SetFill(SelectedColor);
                context.FillText(displayText[selStart..selEnd], new Point(textX + selStartX, textY));

                // Text after selection
                if (selEnd < displayText.Length)
                {
                    context.SetFill(TextColor);
                    context.FillText(displayText[selEnd..], new Point(textX + (allWidth - rightWidth), textY));
                }
            }
            else
            {
                context.SetFill(TextColor);
                context.FillText(displayText, new Point(textX, textY));
            }

            // Caret (1 px vertical line at cursor position)
            if (IsFocused && CaretVisible && sel.IsEmpty)
            {
                var cursorPos = (int)sel.Start;
                var caretOffset = cursorPos > 0
                    ? context.MeasureText(displayText[..cursorPos]).Size.Width
                    : (nfloat)0;
                context.SetFill(TextColor);
                context.FillRect(new Rect(textX + caretOffset, frame.Y + Padding, 1, frame.Height - Padding * 2));
            }
        }

        return guide;
    }
}
