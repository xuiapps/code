// File: Xui/Core/UI/Layers/FocusBorderLayer.cs
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Core.UI.Layers;

/// <summary>
/// A single-child container that renders a background and a focus-aware border.
/// Border color switches between <see cref="NormalBorderColor"/> and <see cref="FocusBorderColor"/>
/// depending on <see cref="IsFocused"/>. Set <see cref="IsFocused"/> from the owning
/// <see cref="View"/>'s <c>OnFocus</c> / <c>OnBlur</c> callbacks.
/// </summary>
/// <typeparam name="TChild">Child layer type drawn inside the border.</typeparam>
public struct FocusBorderLayer<TChild> : ILayer
    where TChild : struct, ILayer
{
    /// <summary>Uniform border stroke width in pixels.</summary>
    public nfloat BorderThickness;

    /// <summary>Corner radius for rounded borders; 0 produces sharp corners.</summary>
    public nfloat CornerRadius;

    /// <summary>Background fill drawn inside the border.</summary>
    public Color BackgroundColor;

    /// <summary>Border color when the control does not have keyboard focus.</summary>
    public Color NormalBorderColor;

    /// <summary>Border color when the control has keyboard focus.</summary>
    public Color FocusBorderColor;

    /// <summary>
    /// Whether the owning view currently holds keyboard focus.
    /// Set to <c>true</c> in <c>OnFocus</c> and <c>false</c> in <c>OnBlur</c>.
    /// </summary>
    public bool IsFocused;

    /// <summary>The single child layer rendered inside the border.</summary>
    public TChild Child;

    public LayoutGuide Update(LayoutGuide guide)
    {
        var t = BorderThickness;
        var t2 = t * 2;

        if (guide.IsMeasure)
        {
            var inner = guide;
            inner.AvailableSize = Size.Max(
                Size.Empty,
                new Size(guide.AvailableSize.Width - t2, guide.AvailableSize.Height - t2));
            inner = Child.Update(inner);
            guide.DesiredSize = new Size(inner.DesiredSize.Width + t2, inner.DesiredSize.Height + t2);
        }

        if (guide.IsArrange)
        {
            var inner = guide;
            var r = guide.ArrangedRect;
            inner.ArrangedRect = new Rect(r.X + t, r.Y + t, r.Width - t2, r.Height - t2);
            Child.Update(inner);
        }

        if (guide.IsRender)
        {
            var context = guide.RenderContext!;
            var r = guide.ArrangedRect;
            var innerRect = new Rect(r.X + t, r.Y + t, r.Width - t2, r.Height - t2);

            // Background
            if (!BackgroundColor.IsTransparent)
            {
                if (CornerRadius > 0)
                {
                    context.BeginPath();
                    nfloat innerRadius = CornerRadius > t ? CornerRadius - t : (nfloat)0;
                    context.RoundRect(innerRect, innerRadius);
                    context.SetFill(BackgroundColor);
                    context.Fill();
                }
                else
                {
                    context.SetFill(BackgroundColor);
                    context.FillRect(innerRect);
                }
            }

            // Border (color depends on focus state)
            var borderColor = IsFocused ? FocusBorderColor : NormalBorderColor;
            if (!borderColor.IsTransparent && t > 0)
            {
                nfloat half = t / 2;
                if (CornerRadius > 0)
                {
                    nfloat strokeRadius = CornerRadius > half ? CornerRadius - half : (nfloat)0;
                    context.BeginPath();
                    context.RoundRect(new Rect(r.X + half, r.Y + half, r.Width - t, r.Height - t), strokeRadius);
                    context.LineWidth = t;
                    context.SetStroke(borderColor);
                    context.Stroke();
                }
                else
                {
                    context.LineWidth = t;
                    context.SetStroke(borderColor);
                    context.StrokeRect(new Rect(r.X + half, r.Y + half, r.Width - t, r.Height - t));
                }
            }

            // Child inside the border
            var inner = guide;
            inner.ArrangedRect = innerRect;
            Child.Update(inner);
        }

        if (guide.IsAnimate && !guide.IsRender && !guide.IsArrange && !guide.IsMeasure)
        {
            Child.Update(guide);
        }

        return guide;
    }
}
