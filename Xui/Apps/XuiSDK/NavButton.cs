using Xui.Core.Animation;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using Xui.Apps.XuiSDK.Icons;

namespace Xui.Apps.XuiSDK;

public class NavButton : View
{
    private bool isHovered;
    private bool isPressed;
    private bool isSelected;

    // Icon selection animation (SmoothDamp driven)
    private nfloat iconSelected;
    private nfloat iconSelectedVelocity;

    // Icon bounce animation
    private nfloat iconBounce = 1;
    private bool iconAnimating;

    // Selection glow animation (bright flash that fades after selection)
    private nfloat selectionGlow;

    // Animation time tracking (initialized to start of time so first frame snaps to final state)
    private TimeSpan lastAnimTime = TimeSpan.Zero;

    // Hover background animation
    private nfloat hoverProgress;

    public string Text { get; set; } = "";
    public INavIcon? NavIcon { get; set; }
    public Action? OnClick { get; set; }

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            if (isSelected != value)
            {
                isSelected = value;
                if (lastAnimTime == TimeSpan.Zero)
                {
                    // No animation frame has run yet — snap to final state
                    iconSelected = value ? 1 : 0;
                    iconSelectedVelocity = 0;
                    selectionGlow = 0;
                    iconBounce = 1;
                    iconAnimating = false;
                }
                else if (value)
                {
                    iconBounce = 0;
                    iconAnimating = true;
                    selectionGlow = 1;
                }
                RequestAnimationFrame();
                InvalidateRender();
            }
        }
    }

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        return new Size(200, 36);
    }

    protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
    {
        nfloat dt = (nfloat)(currentTime - lastAnimTime).TotalSeconds;
        lastAnimTime = currentTime;
        bool needsMore = false;

        // Icon bounce animation (quick spring effect on select)
        if (iconAnimating)
        {
            iconBounce += dt * 4f;
            if (iconBounce >= 1)
            {
                iconBounce = 1;
                iconAnimating = false;
            }
            else
            {
                needsMore = true;
            }
            InvalidateRender();
        }

        // Icon selection animation (SmoothDamp for stroke/fill morph)
        nfloat iconTarget = isSelected ? 1 : 0;
        if (nfloat.Abs(iconSelected - iconTarget) > 0.001f || nfloat.Abs(iconSelectedVelocity) > 0.001f)
        {
            iconSelected = Easing.SmoothDamp(iconSelected, iconTarget, ref iconSelectedVelocity, 0.1f, nfloat.PositiveInfinity, dt);
            if (nfloat.Abs(iconSelected - iconTarget) > 0.001f)
                needsMore = true;
            else
            {
                iconSelected = iconTarget;
                iconSelectedVelocity = 0;
            }
            InvalidateRender();
        }

        // Selection glow decay
        if (selectionGlow > 0.001f)
        {
            selectionGlow = nfloat.Max(selectionGlow - dt * 1.8f, 0);
            if (selectionGlow > 0.001f)
                needsMore = true;
            else
                selectionGlow = 0;
            InvalidateRender();
        }

        // Hover fade animation
        nfloat hoverTarget = isHovered ? 1 : 0;
        if (hoverProgress != hoverTarget)
        {
            nfloat speed = 8f;
            if (hoverProgress < hoverTarget)
                hoverProgress = nfloat.Min(hoverProgress + dt * speed, hoverTarget);
            else
                hoverProgress = nfloat.Max(hoverProgress - dt * speed, hoverTarget);

            if (hoverProgress != hoverTarget)
                needsMore = true;

            InvalidateRender();
        }

        if (needsMore)
            RequestAnimationFrame();
    }

    protected override void RenderCore(IContext context)
    {
        var rect = this.Frame;

        // Background
        if (iconSelected > 0.01f)
        {
            // Dimmed logo gradient colors (resting state)
            // Logo: 0xD642CD → 0x8A05FF, dimmed ~60%
            nfloat glow = Easing.EaseOutCubic(selectionGlow);
            byte r0 = (byte)nfloat.Lerp(0x80, 0xD6, glow);
            byte g0 = (byte)nfloat.Lerp(0x28, 0x42, glow);
            byte b0 = (byte)nfloat.Lerp(0x7A, 0xCD, glow);
            byte r1 = (byte)nfloat.Lerp(0x52, 0x8A, glow);
            byte g1 = (byte)nfloat.Lerp(0x03, 0x05, glow);
            byte b1 = (byte)nfloat.Lerp(0x99, 0xFF, glow);
            byte alpha = (byte)(iconSelected * 0xFF);

            var c0 = new Color(r0, g0, b0, alpha);
            var c1 = new Color(r1, g1, b1, alpha);

            context.Save();
            context.BeginPath();
            context.RoundRect(rect, 5);
            context.Clip();
            context.SetFill(new LinearGradient(
                start: rect.TopLeft,
                end: rect.BottomRight,
                gradient: [new(0, c0), new(1, c1)]));
            context.BeginPath();
            context.Rect(rect);
            context.Fill();
            context.Restore();
        }
        else if (hoverProgress > 0.01f || isPressed)
        {
            nfloat ease = Easing.EaseOutCubic(hoverProgress);
            byte alpha = isPressed ? (byte)0x18 : (byte)(ease * 0x0F);
            context.SetFill(new Color((uint)(0x000000_00 | alpha)));
            context.BeginPath();
            context.RoundRect(rect, 5);
            context.Fill();
        }

        // SVG Icon with bounce + selection animation
        if (NavIcon != null)
        {
            nfloat iconOffsetY = 0;
            if (iconAnimating || iconBounce < 1)
            {
                nfloat t = Easing.EaseOutElastic(iconBounce);
                iconOffsetY = (1 - t) * -4;
            }

            NavIcon.Selected = iconSelected;
            NavIcon.Color = new Color(0x404040FF);
            NavIcon.SelectedColor = Colors.White;

            context.Save();
            context.Translate((rect.X + 20, rect.Y + rect.Height / 2 + iconOffsetY));
            NavIcon.Render(context);
            context.Restore();
        }

        // Label
        context.SetFont(new Font(13, ["Segoe UI"], fontWeight: FontWeight.Normal));
        context.TextBaseline = TextBaseline.Middle;
        context.TextAlign = TextAlign.Left;
        context.SetFill(isSelected ? Colors.White : new Color(0x1A1A1AFF));
        context.FillText(Text, new Point(rect.X + 38, rect.Y + rect.Height / 2));
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (phase != EventPhase.Bubble)
            return;

        switch (e.Type)
        {
            case PointerEventType.Enter:
                isHovered = true;
                RequestAnimationFrame();
                break;

            case PointerEventType.Leave:
                isHovered = false;
                isPressed = false;
                RequestAnimationFrame();
                break;

            case PointerEventType.Down:
                isPressed = true;
                InvalidateRender();
                break;

            case PointerEventType.Up:
                if (isPressed && this.Frame.Contains(e.State.Position))
                {
                    OnClick?.Invoke();
                }
                isPressed = false;
                InvalidateRender();
                break;
        }
    }
}
