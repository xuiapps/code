using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Animation;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Middleware.Emulator.Devices;

namespace Xui.Middleware.Emulator.Actual;

internal sealed class EmulatorChromeRenderer
{
    public void Render(
        IContext ctx,
        in EmulatorGeometry geometry,
        in Rect hostRect,
        DeviceProfile device,
        in EmulatorStatusBarStyle statusBarStyle,
        IClock clock)
    {
        // Outer device frame stroke
        ctx.BeginPath();
        ctx.RoundRect(
            rect: geometry.EmulatorRect.Expand(geometry.BorderWidth * 0.5f),
            radius: geometry.ScreenCornerRadius + geometry.BorderWidth * 0.5f);
        ctx.SetStroke(new Color(0x111111FF));
        ctx.LineWidth = geometry.BorderWidth;
        ctx.Stroke();

        ctx.BeginPath();
        ctx.RoundRect(
            rect: geometry.EmulatorRect.Expand(geometry.BorderWidth - geometry.BorderOutline * 0.5f),
            radius: geometry.ScreenCornerRadius + geometry.BorderWidth * 0.5f + 0.25f * geometry.BorderOutline);
        ctx.SetStroke(new Color(0x444444FF));
        ctx.LineWidth = geometry.BorderOutline;
        ctx.Stroke();

        NFloat phoneToTabletT = Easing.Normalize(hostRect.Width, 500, 575);
        var iconTop = geometry.EmulatorRect.Y + 12f;
        var cutoutRect = new Rect(
            geometry.EmulatorRect.X + device.NotchFrame.X,
            geometry.EmulatorRect.Y + device.NotchFrame.Y,
            device.NotchFrame.Width,
            device.NotchFrame.Height);
        RenderCameraCutout(ctx, device, cutoutRect);

        NFloat clockX = NFloat.Lerp(
            (geometry.EmulatorRect.Center.X - 22f) / 2,
            (300 / 2f - 22f) / 2,
            Easing.EaseInOutSine(phoneToTabletT));
        EmulatorWindow.ClockIcon.Instance.Render(ctx, (clockX, iconTop + 6f), clock.Now);

        NFloat instrumentsX = NFloat.Lerp(
            geometry.EmulatorRect.Center.X + 45f + (geometry.EmulatorRect.Center.X - 45f - 22f) / 2f,
            hostRect.Width - 80f,
            Easing.EaseInOutSine(phoneToTabletT));

        EmulatorWindow.SignalStrengthIcon.Instance.Render(
            ctx,
            (instrumentsX - 12f - 8f, iconTop + 19.5f),
            statusBarStyle.SignalStrength ?? 0.67f);
        EmulatorWindow.BatteryIcon.Instance.Render(
            ctx,
            (instrumentsX - 8f, iconTop + 8.5f),
            statusBarStyle.BatteryLevel ?? 0.65f);
        EmulatorWindow.FiveGIcon.Instance.Render(
            ctx,
            (instrumentsX + 36f - 8f, iconTop + 6f),
            statusBarStyle.NetworkText ?? "5G");

        // Menu Handle
        EmulatorWindow.MenuHandle.Instance.Render(ctx, (
            geometry.EmulatorRect.Center.X,
            geometry.EmulatorRect.Bottom - 3f
        ));
    }

    private static void RenderCameraCutout(IContext ctx, DeviceProfile device, Rect cutoutRect)
    {
        if (device.NotchType == NotchType.None || cutoutRect.Width <= 0 || cutoutRect.Height <= 0)
            return;

        ctx.BeginPath();
        switch (device.NotchType)
        {
            case NotchType.PinHole:
            {
                var radius = NFloat.Min(cutoutRect.Width, cutoutRect.Height) * 0.5f;
                ctx.Ellipse(cutoutRect.Center, radius, radius, 0, 0, NFloat.Pi * 2f, Winding.ClockWise);
                break;
            }
            case NotchType.DynamicIsland:
            {
                var radius = cutoutRect.Height * 0.5f;
                ctx.RoundRect(cutoutRect, radius);
                break;
            }
            case NotchType.Waterdrop:
                RenderWaterdropCutout(ctx, cutoutRect);
                break;
            case NotchType.Notch:
                RenderNotchCutout(ctx, cutoutRect);
                break;
            default:
            {
                var radius = NFloat.Min(cutoutRect.Height * 0.5f, 10f);
                ctx.RoundRect(cutoutRect, radius);
                break;
            }
        }

        ctx.SetFill(0x111111FF);
        ctx.Fill();
    }

    private static void RenderNotchCutout(IContext ctx, Rect cutoutRect)
    {
        var bottomRadius = NFloat.Min(cutoutRect.Height * 0.5f, cutoutRect.Width * 0.18f);
        ctx.RoundRect(cutoutRect, new CornerRadius(0, 0, bottomRadius, bottomRadius));
    }

    private static void RenderWaterdropCutout(IContext ctx, Rect cutoutRect)
    {
        var shoulderY = cutoutRect.Top + cutoutRect.Height * 0.22f;
        var centerX = cutoutRect.Center.X;
        var lobeControlX = cutoutRect.Width * 0.28f;

        ctx.MoveTo((cutoutRect.Left, cutoutRect.Top));
        ctx.LineTo((cutoutRect.Right, cutoutRect.Top));
        ctx.LineTo((cutoutRect.Right, shoulderY));
        ctx.CurveTo(
            (cutoutRect.Right, cutoutRect.Bottom),
            (centerX + lobeControlX, cutoutRect.Bottom),
            (centerX, cutoutRect.Bottom));
        ctx.CurveTo(
            (centerX - lobeControlX, cutoutRect.Bottom),
            (cutoutRect.Left, cutoutRect.Bottom),
            (cutoutRect.Left, shoulderY));
        ctx.ClosePath();
    }
}
