using Xui.Core.Animation;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;

namespace Xui.Apps.XuiSDK;

public class FeatureCard : View
{
    private nfloat hoverProgress;
    private bool isHovered;
    private bool isPressed;

    public string Title { get; set; } = "";
    public string Line1 { get; set; } = "";
    public string Line2 { get; set; } = "";

    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        return new Size(180, 120);
    }

    protected override void AnimateCore(TimeSpan previousTime, TimeSpan currentTime)
    {
        nfloat target = isHovered ? 1 : 0;
        nfloat dt = (nfloat)(currentTime - previousTime).TotalSeconds;
        nfloat speed = 6f;

        nfloat prev = hoverProgress;
        if (hoverProgress < target)
            hoverProgress = nfloat.Min(hoverProgress + dt * speed, target);
        else
            hoverProgress = nfloat.Max(hoverProgress - dt * speed, target);

        if (hoverProgress != target)
            RequestAnimationFrame();

        if (prev != hoverProgress)
            InvalidateRender();
    }

    protected override void RenderCore(IContext context)
    {
        var rect = this.Frame;
        nfloat ease = Easing.EaseOutCubic(hoverProgress);

        // Elevation: slight scale + shadow on hover
        nfloat scale = 1 + ease * 0.02f;
        nfloat cx = rect.X + rect.Width / 2;
        nfloat cy = rect.Y + rect.Height / 2;
        nfloat sw = rect.Width * scale;
        nfloat sh = rect.Height * scale;
        var drawRect = new Rect(cx - sw / 2, cy - sh / 2, sw, sh);

        // Shadow on hover
        if (hoverProgress > 0.01f)
        {
            nfloat shadowAlpha = ease * 0.12f;
            byte a = (byte)(shadowAlpha * 255);
            context.SetFill(new Color((uint)(0x000000_00 | a)));
            context.BeginPath();
            context.RoundRect(new Rect(drawRect.X + 1, drawRect.Y + 2, drawRect.Width, drawRect.Height), 8);
            context.Fill();
        }

        // Card background - brighter on hover
        byte bgAlpha = (byte)(0x80 + ease * 0x50);
        context.SetFill(new Color((uint)(0xFFFFFF_00 | bgAlpha)));
        context.BeginPath();
        context.RoundRect(drawRect, 8);
        context.Fill();

        // Card border - stronger on hover
        byte borderAlpha = (byte)(0x15 + ease * 0x15);
        context.SetStroke(new Color((uint)(0x000000_00 | borderAlpha)));
        context.LineWidth = 1;
        context.BeginPath();
        context.RoundRect(drawRect, 8);
        context.Stroke();

        // Press feedback
        if (isPressed)
        {
            context.SetFill(new Color(0x00000008));
            context.BeginPath();
            context.RoundRect(drawRect, 8);
            context.Fill();
        }

        // Accent top bar on hover
        if (hoverProgress > 0.01f)
        {
            byte accentAlpha = (byte)(ease * 255);
            context.SetFill(new Color((uint)(0x0078D4_00 | accentAlpha)));
            context.BeginPath();
            context.RoundRect(new Rect(drawRect.X + 16, drawRect.Y, drawRect.Width - 32, 3), 1.5f);
            context.Fill();
        }

        // Card title
        context.SetFont(new Font(14, ["Segoe UI"], fontWeight: FontWeight.SemiBold));
        context.TextBaseline = TextBaseline.Top;
        context.TextAlign = TextAlign.Left;
        context.SetFill(new Color(0x1A1A1AFF));
        context.FillText(Title, new Point(drawRect.X + 16, drawRect.Y + 16));

        // Description line 1
        context.SetFont(new Font(12, ["Segoe UI"]));
        context.SetFill(new Color(0x606060FF));
        context.FillText(Line1, new Point(drawRect.X + 16, drawRect.Y + 40));

        // Description line 2
        if (Line2.Length > 0)
        {
            context.FillText(Line2, new Point(drawRect.X + 16, drawRect.Y + 58));
        }
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
                isPressed = false;
                InvalidateRender();
                break;
        }
    }
}
