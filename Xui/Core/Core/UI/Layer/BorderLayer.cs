using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI.Input;

namespace Xui.Core.UI.Layer;

/// <summary>
/// A layer that draws a background fill, a border stroke, and optional padding around
/// a child layer. Rendering logic is identical to <see cref="Xui.Core.UI.Border"/> but
/// expressed as a composable struct with no heap allocation.
/// </summary>
/// <typeparam name="TChild">The inner layer that receives the inset rectangle.</typeparam>
public struct BorderLayer<TChild> : ILayer<View>
    where TChild : struct, ILayer<View>
{
    /// <summary>The inner layer rendered inside the border and padding.</summary>
    public TChild Child;

    /// <summary>Per-side border thickness.</summary>
    public Frame BorderThickness;

    /// <summary>Corner radius for the background fill and border stroke.</summary>
    public CornerRadius CornerRadius;

    /// <summary>Background fill color.</summary>
    public Color BackgroundColor;

    /// <summary>Border stroke color.</summary>
    public Color BorderColor;

    /// <summary>Padding between the border edge and the child layer.</summary>
    public Frame Padding;

    private readonly Frame Inset => BorderThickness + Padding;

    /// <inheritdoc/>
    public Size Measure(View view, Size availableSize, IMeasureContext context)
    {
        Size childSize = Child.Measure(view, Size.Max(Size.Empty, availableSize - Inset), context);
        return childSize + Inset;
    }

    /// <inheritdoc/>
    public void Arrange(View view, Rect rect, IMeasureContext context)
        => Child.Arrange(view, rect - Padding - BorderThickness, context);

    /// <inheritdoc/>
    public void Render(View view, IContext context)
    {
        Rect frame = view.Frame;

        if (!BackgroundColor.IsTransparent)
        {
            if (CornerRadius.IsZero)
            {
                context.SetFill(BackgroundColor);
                context.FillRect(frame - BorderThickness);
            }
            else if (BorderThickness.IsUniform)
            {
                context.BeginPath();
                context.RoundRect(
                    frame - BorderThickness,
                    CornerRadius.Max(CornerRadius.Zero, CornerRadius - BorderThickness.Left));
                context.SetFill(BackgroundColor);
                context.Fill();
            }
        }

        Child.Render(view, context);

        if (!BorderColor.IsTransparent && !BorderThickness.IsZero)
        {
            if (BorderThickness.IsUniform)
            {
                nfloat half = BorderThickness.Left * (nfloat).5;
                if (CornerRadius.IsZero)
                {
                    context.LineWidth = BorderThickness.Left;
                    context.SetStroke(BorderColor);
                    context.StrokeRect(frame - half);
                }
                else
                {
                    context.BeginPath();
                    context.RoundRect(frame - half, CornerRadius - half);
                    context.LineWidth = BorderThickness.Left;
                    context.SetStroke(BorderColor);
                    context.Stroke();
                }
            }
        }
    }

    /// <inheritdoc/>
    public void Update(View view, ref LayoutGuide guide)
    {
        if (guide.IsAnimate) Animate(view, guide.PreviousTime, guide.CurrentTime);
        if (guide.IsMeasure) guide.DesiredSize = Measure(view, guide.AvailableSize, guide.MeasureContext!);
        if (guide.IsArrange) Arrange(view, guide.ArrangedRect, guide.MeasureContext!);
        if (guide.IsRender)  Render(view, guide.RenderContext!);
    }

    /// <inheritdoc/>
    public void Animate(View view, TimeSpan previousTime, TimeSpan currentTime)
        => Child.Animate(view, previousTime, currentTime);

    /// <inheritdoc/>
    public void OnPointerEvent(View view, ref PointerEventRef e, EventPhase phase)
        => Child.OnPointerEvent(view, ref e, phase);

    /// <inheritdoc/>
    public void OnKeyDown(View view, ref KeyEventRef e)
        => Child.OnKeyDown(view, ref e);

    /// <inheritdoc/>
    public void OnChar(View view, ref KeyEventRef e)
        => Child.OnChar(view, ref e);

    /// <inheritdoc/>
    public void OnFocus(View view) => Child.OnFocus(view);

    /// <inheritdoc/>
    public void OnBlur(View view) => Child.OnBlur(view);
}
