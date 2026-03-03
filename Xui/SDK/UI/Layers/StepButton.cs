// File: Xui/SDK/UI/Layers/StepButton.cs
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layers;

namespace Xui.SDK.UI.Layers;

/// <summary>
/// Leaf layer for the decrement (−) and increment (+) buttons in a <see cref="Xui.SDK.UI.NumberBox"/>.
/// Renders a rounded rectangle button with a centered label, adapting color based on
/// <see cref="Pressed"/> and <see cref="Disabled"/> state.
/// </summary>
public struct StepButton : ILeaf
{
    /// <summary>Button label, typically "−" or "+".</summary>
    public string? Label;

    /// <summary>Whether the button is currently pressed (pointer-down over the button).</summary>
    public bool Pressed;

    /// <summary>Whether the button is disabled (Value is at Min or Max boundary).</summary>
    public bool Disabled;

    public Color NormalColor;
    public Color PressedColor;
    public Color DisabledColor;
    public Color TextColor;
    public Color DisabledTextColor;
    public nfloat CornerRadius;
    public nfloat FontSize;
    public string[]? FontFamily;

    /// <summary>Fixed width of the button; height fills the available row height.</summary>
    public nfloat PreferredWidth;

    public LayoutGuide Update(LayoutGuide guide)
    {
        if (guide.IsMeasure)
        {
            // Use available height when finite (fills a fixed-height row);
            // fall back to FontSize * 2 when the parent stack is unbounded.
            var h = guide.AvailableSize.Height;
            if (!nfloat.IsFinite(h)) h = FontSize * 2;
            guide.DesiredSize = new Size(PreferredWidth, h);
        }

        if (guide.IsRender)
        {
            var ctx = guide.RenderContext!;
            var r = guide.ArrangedRect;

            var bgColor = Disabled ? DisabledColor : Pressed ? PressedColor : NormalColor;
            if (!bgColor.IsTransparent)
            {
                if (CornerRadius > 0)
                {
                    ctx.BeginPath();
                    ctx.RoundRect(r, CornerRadius);
                    ctx.SetFill(bgColor);
                    ctx.Fill();
                }
                else
                {
                    ctx.SetFill(bgColor);
                    ctx.FillRect(r);
                }
            }

            if (!string.IsNullOrEmpty(Label))
            {
                ctx.SetFont(new Font
                {
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    FontWeight = FontWeight.Bold,
                });
                ctx.TextBaseline = TextBaseline.Middle;
                ctx.TextAlign = TextAlign.Center;
                ctx.SetFill(Disabled ? DisabledTextColor : TextColor);
                ctx.FillText(Label, new Point(r.X + r.Width / 2, r.Y + r.Height / 2));
            }
        }

        return guide;
    }
}
