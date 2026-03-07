using System.Runtime.InteropServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Core.UI.Layer;

namespace Xui.Apps.TestApp.Pages.Layers.Tests;

/// <summary>
/// Demo-only leaf layer that renders a clickable button with an optional label,
/// margin, corner radius, and hover/pressed visual states.
/// When <see cref="Visible"/> is false, <see cref="Measure"/> returns zero width
/// so the docked slot collapses.
/// </summary>
public struct ButtonLayer : ILayer<View>
{
    /// <summary>Label text drawn centered in the button.</summary>
    public string? Label;

    /// <summary>Outer margin: shrinks the visible button rect within the docked slot.</summary>
    public NFloat Margin;

    /// <summary>Corner radius of the visible button rect.</summary>
    public CornerRadius CornerRadius;

    public Color NormalColor;
    public Color HoverColor;
    public Color PressedColor;
    public Color LabelColor;
    public string[]? FontFamily;
    public NFloat FontSize;

    /// <summary>
    /// When false, <see cref="Measure"/> returns zero width so the dock slot collapses.
    /// Set to true and call <see cref="View.InvalidateMeasure"/> to make the button appear.
    /// </summary>
    public bool Visible;

    /// <summary>Invoked when the button is tapped/clicked.</summary>
    public Action? OnClick;

    /// <summary>
    /// Wire to the owning view's <c>InvalidateRender</c> from the LayerView subclass constructor.
    /// Required because <c>View.InvalidateRender</c> is protected internal and not callable
    /// from an external-assembly layer struct.
    /// </summary>
    public Action? RequestRedraw;

    private bool hover;
    private bool pressed;
    private Rect frame;

    // ── ILayer<View> ─────────────────────────────────────────────────────

    public void Update(View view, ref LayoutGuide guide)
    {
        if (guide.IsAnimate) Animate(view, guide.PreviousTime, guide.CurrentTime);
        if (guide.IsMeasure) guide.DesiredSize = Measure(view, guide.AvailableSize, guide.MeasureContext!);
        if (guide.IsArrange) Arrange(view, guide.ArrangedRect, guide.MeasureContext!);
        if (guide.IsRender)  Render(view, guide.RenderContext!);
    }

    public Size Measure(View view, Size available, IMeasureContext ctx)
    {
        if (!Visible) return new Size(0, available.Height);
        NFloat side = NFloat.IsFinite(available.Height) ? available.Height
                    : FontSize > 0 ? FontSize * 2 : (NFloat)30;
        return new Size(side, side);
    }

    public void Arrange(View view, Rect rect, IMeasureContext ctx)
    {
        frame = rect;
    }

    public void Render(View view, IContext ctx)
    {
        if (!Visible) return;

        var btn = BtnRect();
        var bg = pressed ? PressedColor : hover ? HoverColor : NormalColor;

        if (!bg.IsTransparent)
        {
            if (CornerRadius.IsZero)
            {
                ctx.SetFill(bg);
                ctx.FillRect(btn);
            }
            else
            {
                ctx.BeginPath();
                ctx.RoundRect(btn, CornerRadius);
                ctx.SetFill(bg);
                ctx.Fill();
            }
        }

        if (!string.IsNullOrEmpty(Label))
        {
            ctx.SetFont(new Font
            {
                FontFamily = FontFamily ?? ["Inter"],
                FontSize   = FontSize > 0 ? FontSize : 13,
                FontWeight = FontWeight.Normal,
            });
            ctx.TextAlign    = TextAlign.Center;
            ctx.TextBaseline = TextBaseline.Middle;
            ctx.SetFill(LabelColor);
            ctx.FillText(Label, new Point(btn.X + btn.Width / 2, btn.Y + btn.Height / 2));
        }
    }

    public void Animate(View view, TimeSpan p, TimeSpan c) { }

    public void OnPointerEvent(View view, ref PointerEventRef e, EventPhase phase)
    {
        if (!Visible || phase != EventPhase.Bubble)
            return;

        var btn     = BtnRect();
        var pos     = e.State.Position;
        bool inBtn  = pos.X >= btn.X && pos.X < btn.X + btn.Width
                   && pos.Y >= btn.Y && pos.Y < btn.Y + btn.Height;

        switch (e.Type)
        {
            case PointerEventType.Enter:
                hover = inBtn;
                RequestRedraw?.Invoke();
                break;

            case PointerEventType.Leave:
                hover   = false;
                pressed = false;
                RequestRedraw?.Invoke();
                break;

            case PointerEventType.Move:
                bool was = hover;
                hover = inBtn;
                if (hover != was) RequestRedraw?.Invoke();
                break;

            case PointerEventType.Down when inBtn:
                pressed = true;
                view.CapturePointer(e.PointerId);
                RequestRedraw?.Invoke();
                break;

            case PointerEventType.Up when pressed:
                pressed = false;
                view.ReleasePointer(e.PointerId);
                if (inBtn) OnClick?.Invoke();
                RequestRedraw?.Invoke();
                break;
        }
    }

    public void OnKeyDown(View view, ref KeyEventRef e) { }
    public void OnChar(View view, ref KeyEventRef e)    { }
    public void OnFocus(View view)                      { }
    public void OnBlur(View view)                       { }

    private Rect BtnRect() => new Rect(
        frame.X + Margin,
        frame.Y + Margin,
        frame.Width  - Margin * 2,
        frame.Height - Margin * 2);
}
