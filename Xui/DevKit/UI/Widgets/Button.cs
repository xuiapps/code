using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.DevKit.UI.Design;

namespace Xui.DevKit.UI.Widgets;

/// <summary>
/// A filled button that consumes design system tokens for colors, shape, and typography.
/// Supports hover and pressed visual states.
/// </summary>
public class Button : View
{
    private bool hover;
    private bool pressed;

    private Color fillColor;
    private Color textColor;
    private Color hoverColor;
    private Color pressedColor;
    private CornerRadius cornerRadius;
    private nfloat paddingH;
    private nfloat paddingV;
    private TextStyle textStyle;

    /// <summary>The button label text.</summary>
    public string Text { get; set; } = "";

    /// <summary>Invoked when the button is clicked.</summary>
    public Action? Clicked { get; set; }

    /// <inheritdoc/>
    public override int Count => 0;

    /// <inheritdoc/>
    public override View this[int index] => throw new IndexOutOfRangeException();

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

        var primary = ds.Colors.Primary;
        fillColor = primary.Background;
        textColor = primary.Foreground;
        hoverColor = primary.Ramp[0.48f];
        pressedColor = primary.Ramp[0.35f];
        cornerRadius = ds.Shape.Full;
        paddingH = ds.Spacing.L;
        paddingV = ds.Spacing.S;
        textStyle = ds.Typography.Label.L;
    }

    /// <inheritdoc/>
    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        context.SetFont(new Font(
            textStyle.FontSize,
            [textStyle.FontFamily],
            textStyle.FontWeight,
            textStyle.FontStyle
        ));
        var textSize = context.MeasureText(Text).Size;
        return new Size(
            textSize.Width + paddingH * 2,
            textSize.Height + paddingV * 2
        );
    }

    /// <inheritdoc/>
    protected override void RenderCore(IContext context)
    {
        var fill = pressed ? pressedColor : hover ? hoverColor : fillColor;

        context.BeginPath();
        context.RoundRect(this.Frame, cornerRadius);
        context.SetFill(fill);
        context.Fill();

        context.SetFont(new Font(
            textStyle.FontSize,
            [textStyle.FontFamily],
            textStyle.FontWeight,
            textStyle.FontStyle
        ));
        context.TextBaseline = TextBaseline.Top;
        context.SetFill(textColor);

        var textMeasure = context.MeasureText(Text);
        var tx = this.Frame.X + (this.Frame.Width - textMeasure.Size.Width) / 2;
        var ty = this.Frame.Y + (this.Frame.Height - textMeasure.Size.Height) / 2;
        context.FillText(Text, new Point(tx, ty));
    }

    /// <inheritdoc/>
    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.Type == PointerEventType.Enter)
        {
            hover = true;
            this.InvalidateRender();
        }
        else if (e.Type == PointerEventType.Leave)
        {
            hover = false;
            this.InvalidateRender();
        }
        else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
        {
            this.CapturePointer(e.PointerId);
            pressed = true;
            this.InvalidateRender();
        }
        else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
        {
            this.ReleasePointer(e.PointerId);
            if (pressed && this.Frame.Contains(e.State.Position))
                Clicked?.Invoke();
            pressed = false;
            this.InvalidateRender();
        }

        base.OnPointerEvent(ref e, phase);
    }
}
