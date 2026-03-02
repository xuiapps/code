// File: Xui/SDK/UI/Layers/NumberCenterLayer.cs
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layers;

namespace Xui.SDK.UI.Layers;

/// <summary>
/// Leaf layer for the center value area of a <see cref="NumberBox"/>.
/// Renders text centered both horizontally and vertically.
/// When focused supports a select-all highlight (blue rect covering all text) and
/// a blinking insertion caret at the end of the current text.
/// </summary>
public struct NumberCenterLayer : ILeaf
{
    public string? Text;

    public string[]? FontFamily;
    public nfloat FontSize;

    public Color TextColor;

    public bool IsFocused;

    /// <summary>Toggled by the owning view's animation loop.</summary>
    public bool CaretVisible;

    /// <summary>
    /// When true the entire text area is shown as selected (blue rect / white text).
    /// Used for the "select all on focus" state.
    /// </summary>
    public bool SelectAll;

    public Color SelectionBackgroundColor;
    public Color SelectedTextColor;

    private Font GetFont() => new Font { FontFamily = FontFamily, FontSize = FontSize };

    public LayoutGuide Update(LayoutGuide guide)
    {
        var text = Text ?? string.Empty;

        if (guide.IsMeasure)
        {
            var ctx = guide.MeasureContext!;
            ctx.SetFont(GetFont());
            // Fill the available width; height comes from the text metrics.
            var metrics = ctx.MeasureText(text.Length > 0 ? text : "0");
            guide.DesiredSize = new Size(guide.AvailableSize.Width, metrics.Size.Height);
        }

        if (guide.IsRender)
        {
            var ctx = guide.RenderContext!;
            var r = guide.ArrangedRect;
            ctx.SetFont(GetFont());

            // Center text horizontally and vertically.
            var metrics = ctx.MeasureText(text.Length > 0 ? text : "0");
            var textW = ctx.MeasureText(text).Size.Width;
            nfloat textX = r.X + (r.Width - textW) / 2;
            nfloat textY = r.Y + (r.Height - metrics.Size.Height) / 2;

            if (IsFocused && SelectAll)
            {
                // Selection rect covering the text (2 px horizontal margin on each side)
                nfloat hMargin = 4;
                var selRect = new Rect(
                    textX - hMargin,
                    r.Y + 2,
                    textW + hMargin * 2,
                    r.Height - 4);
                ctx.SetFill(SelectionBackgroundColor);
                ctx.FillRect(selRect);

                ctx.TextBaseline = TextBaseline.Top;
                ctx.TextAlign = TextAlign.Left;
                ctx.SetFill(SelectedTextColor);
                ctx.FillText(text, new Point(textX, textY));
            }
            else
            {
                ctx.TextBaseline = TextBaseline.Top;
                ctx.TextAlign = TextAlign.Left;
                ctx.SetFill(TextColor);
                ctx.FillText(text, new Point(textX, textY));

                // Blinking caret after the text when focused (edit mode)
                if (IsFocused && CaretVisible && !SelectAll)
                {
                    nfloat caretX = textX + textW;
                    ctx.SetFill(TextColor);
                    ctx.FillRect(new Rect(caretX, r.Y + 2, 1, r.Height - 4));
                }
            }
        }

        return guide;
    }
}
