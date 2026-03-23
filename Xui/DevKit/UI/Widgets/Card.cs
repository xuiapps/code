using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.DevKit.UI.Design;

namespace Xui.DevKit.UI.Widgets;

/// <summary>
/// A card container that uses Surface colors from the design system.
/// Renders a rounded background and arranges a single child with padding.
/// </summary>
public class Card : View
{
    private View? content;

    private Color backgroundColor;
    private Color foregroundColor;
    private CornerRadius cornerRadius;
    private nfloat padding;

    /// <summary>The content view inside the card.</summary>
    public View? Content
    {
        get => content;
        set => this.SetProtectedChild(ref content, value);
    }

    /// <inheritdoc/>
    public override int Count => content is null ? 0 : 1;

    /// <inheritdoc/>
    public override View this[int index] =>
        index == 0 && content is not null ? content : throw new IndexOutOfRangeException();

    /// <inheritdoc/>
    protected override void OnActivate()
    {
        base.OnActivate();
        ApplyDesignSystem();
    }

    private void ApplyDesignSystem()
    {
        var ds = this.GetService(typeof(IDesignSystem)) as IDesignSystem;
        if (ds == null) return;

        backgroundColor = ds.Colors.Surface.Background;
        foregroundColor = ds.Colors.Surface.Foreground;
        cornerRadius = ds.Shape.Large;
        padding = ds.Spacing.L;
    }

    /// <inheritdoc/>
    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        if (content == null)
            return new Size(padding * 2, padding * 2);

        var inner = new Size(
            available.Width - padding * 2,
            available.Height - padding * 2
        );
        var childSize = content.Measure(inner, context);
        return new Size(
            childSize.Width + padding * 2,
            childSize.Height + padding * 2
        );
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        if (content == null) return;

        var inner = new Rect(
            this.Frame.X + padding,
            this.Frame.Y + padding,
            this.Frame.Width - padding * 2,
            this.Frame.Height - padding * 2
        );
        content.Arrange(inner, context);
    }

    /// <inheritdoc/>
    protected override void RenderCore(IContext context)
    {
        context.BeginPath();
        context.RoundRect(this.Frame, cornerRadius);
        context.SetFill(backgroundColor);
        context.Fill();

        if (content != null)
            content.Render(context);
    }
}
