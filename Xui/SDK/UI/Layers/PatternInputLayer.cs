// File: Xui/SDK/UI/Layers/PatternInputLayer.cs
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layers;

namespace Xui.SDK.UI.Layers;

/// <summary>
/// Leaf layer for a pattern-masked input. The <see cref="Pattern"/> string uses
/// <c>#</c> as input-slot placeholders and any other character as a literal separator.
/// Each slot is rendered as a bordered box; typed characters appear centered inside.
/// The active slot shows a blinking caret (empty) or a blinking selection rect (filled).
/// Up to 32 slots are supported.
/// </summary>
public struct PatternInputLayer : ILeaf
{
    // --- Pattern / Value state ---

    public string? Pattern;
    public char[]? Value;

    /// <summary>0-based index of the active input slot.</summary>
    public int CursorSlot;

    /// <summary>Whether the owning view has keyboard focus.</summary>
    public bool IsFocused;

    /// <summary>
    /// Toggled by the owning view's animation loop to produce caret/selection blinking.
    /// When true the caret line (empty slot) or selection rect (filled slot) is visible.
    /// </summary>
    public bool CaretVisible;

    // --- Layout ---

    public nfloat SlotWidth;
    public nfloat SlotHeight;
    public nfloat SlotGap;
    public nfloat SeparatorGap;

    // --- Colors ---

    public Color SlotBackground;
    public Color SlotBorderColor;
    public Color FocusSlotBorderColor;
    public Color TextColor;
    public Color CursorColor;
    public Color SelectionBackgroundColor;
    public Color SelectedTextColor;

    // --- Typography ---

    public nfloat FontSize;
    public string[]? FontFamily;

    // --- Cached geometry (populated in Measure) ---

    private nfloat[] _slotOffsets;

    // ---

    private static Font GetFont(string[]? fontFamily, nfloat fontSize) => new Font
    {
        FontFamily = fontFamily,
        FontSize = fontSize,
    };

    /// <summary>
    /// Returns the index of the slot nearest to <paramref name="px"/> (relative to
    /// the layer's arranged rect origin), or -1 if no slots exist.
    /// Must be called after a Measure pass so slot offsets are populated.
    /// </summary>
    public int HitSlot(nfloat px)
    {
        if (_slotOffsets is null || _slotOffsets.Length == 0) return -1;

        int best = 0;
        nfloat bestDist = nfloat.Abs(px - (_slotOffsets[0] + SlotWidth / 2));

        for (int i = 1; i < _slotOffsets.Length; i++)
        {
            nfloat center = _slotOffsets[i] + SlotWidth / 2;
            nfloat dist = nfloat.Abs(px - center);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = i;
            }
        }

        return best;
    }

    public LayoutGuide Update(LayoutGuide guide)
    {
        var pattern = Pattern ?? string.Empty;

        int slotCount = 0;
        for (int i = 0; i < pattern.Length; i++)
            if (pattern[i] == '#') slotCount++;

        if (guide.IsMeasure)
        {
            var ctx = guide.MeasureContext!;
            ctx.SetFont(GetFont(FontFamily, FontSize));

            nfloat totalWidth = 0;
            int slot = 0;
            _slotOffsets ??= new nfloat[Math.Max(slotCount, 1)];
            if (_slotOffsets.Length != slotCount && slotCount > 0)
                _slotOffsets = new nfloat[slotCount];

            bool prevWasSlot = false;
            for (int i = 0; i < pattern.Length; i++)
            {
                char c = pattern[i];
                if (c == '#')
                {
                    if (prevWasSlot) totalWidth += SlotGap;
                    if (slot < slotCount) _slotOffsets[slot] = totalWidth;
                    totalWidth += SlotWidth;
                    slot++;
                    prevWasSlot = true;
                }
                else
                {
                    nfloat sepW = ctx.MeasureText(c.ToString()).Size.Width + SeparatorGap * 2;
                    if (prevWasSlot) totalWidth += SlotGap;
                    totalWidth += sepW;
                    prevWasSlot = false;
                }
            }

            guide.DesiredSize = new Size(totalWidth, SlotHeight);
        }

        if (guide.IsRender)
        {
            var ctx = guide.RenderContext!;
            var r = guide.ArrangedRect;
            ctx.SetFont(GetFont(FontFamily, FontSize));

            nfloat x = r.X;
            nfloat slotTop = r.Y + (r.Height - SlotHeight) / 2;
            int slot = 0;
            bool prevWasSlot = false;
            nfloat selMargin = 2;

            for (int i = 0; i < pattern.Length; i++)
            {
                char c = pattern[i];
                if (c == '#')
                {
                    if (prevWasSlot) x += SlotGap;

                    var slotRect = new Rect(x, slotTop, SlotWidth, SlotHeight);
                    bool isActive = IsFocused && slot == CursorSlot;
                    char slotChar = (Value != null && slot < Value.Length) ? Value[slot] : '\0';
                    bool isFilled = slotChar != '\0';

                    // Slot background
                    if (!SlotBackground.IsTransparent)
                    {
                        ctx.SetFill(SlotBackground);
                        ctx.FillRect(slotRect);
                    }

                    // Selection rect for active filled slot (blinks via CaretVisible)
                    if (isActive && isFilled && CaretVisible)
                    {
                        var selRect = new Rect(
                            x + selMargin,
                            slotTop + selMargin,
                            SlotWidth - selMargin * 2,
                            SlotHeight - selMargin * 2);
                        ctx.SetFill(SelectionBackgroundColor);
                        ctx.FillRect(selRect);
                    }

                    // Slot border (highlighted for active slot)
                    var borderColor = isActive ? FocusSlotBorderColor : SlotBorderColor;
                    if (!borderColor.IsTransparent)
                    {
                        ctx.LineWidth = 1;
                        ctx.SetStroke(borderColor);
                        ctx.StrokeRect(slotRect);
                    }

                    // Slot character
                    if (isFilled)
                    {
                        ctx.TextBaseline = TextBaseline.Middle;
                        ctx.TextAlign = TextAlign.Center;
                        // Invert text color when selection rect is visible
                        var charColor = (isActive && CaretVisible) ? SelectedTextColor : TextColor;
                        ctx.SetFill(charColor);
                        ctx.FillText(slotChar.ToString(), new Point(x + SlotWidth / 2, slotTop + SlotHeight / 2));
                    }

                    // Blinking caret line for active empty slot
                    if (isActive && !isFilled && CaretVisible)
                    {
                        ctx.SetFill(CursorColor);
                        ctx.FillRect(new Rect(x + SlotWidth / 2, slotTop + 4, 1, SlotHeight - 8));
                    }

                    x += SlotWidth;
                    slot++;
                    prevWasSlot = true;
                }
                else
                {
                    nfloat sepW = ctx.MeasureText(c.ToString()).Size.Width + SeparatorGap * 2;
                    if (prevWasSlot) x += SlotGap;

                    ctx.TextBaseline = TextBaseline.Middle;
                    ctx.TextAlign = TextAlign.Center;
                    ctx.SetFill(TextColor);
                    ctx.FillText(c.ToString(), new Point(x + sepW / 2, slotTop + SlotHeight / 2));

                    x += sepW;
                    prevWasSlot = false;
                }
            }
        }

        return guide;
    }
}
