using System.Runtime.InteropServices;
using Xui.Core.Math2D;
using Xui.Middleware.Emulator.Devices;

namespace Xui.Middleware.Emulator.Actual;

internal readonly struct EmulatorGeometry
{
    public readonly NFloat TitleHeight;
    public readonly NFloat Gap;
    public readonly NFloat BorderWidth;
    public readonly NFloat BorderOutline;
    public readonly NFloat ScreenCornerRadius;
    public readonly Rect TitleRect;
    public readonly Rect EmulatorRect;
    public readonly bool HasHostTitle;

    private EmulatorGeometry(
        NFloat titleHeight,
        NFloat gap,
        NFloat borderWidth,
        NFloat borderOutline,
        NFloat screenCornerRadius,
        Rect titleRect,
        Rect emulatorRect,
        bool hasHostTitle)
    {
        TitleHeight = titleHeight;
        Gap = gap;
        BorderWidth = borderWidth;
        BorderOutline = borderOutline;
        ScreenCornerRadius = screenCornerRadius;
        TitleRect = titleRect;
        EmulatorRect = emulatorRect;
        HasHostTitle = hasHostTitle;
    }

    public static EmulatorGeometry Create(Rect hostRect, DeviceProfile device)
    {
        NFloat titleHeight = 52f;
        NFloat gap = 8f;
        NFloat borderWidth = 8f;
        NFloat borderOutline = 2.5f;
        NFloat corner = device.ScreenCornerRadius;

        var emulatorWidth = device.LogicalResolution.Width;
        var emulatorHeight = device.LogicalResolution.Height;
        var emulatorX = (hostRect.Width - emulatorWidth) * 0.5f;
        var emulatorY = titleHeight + gap;

        var emulatorRect = new Rect(emulatorX, emulatorY, emulatorWidth, emulatorHeight);
        var titleRect = new Rect(0, 0, hostRect.Width, titleHeight);

        return new EmulatorGeometry(
            titleHeight,
            gap,
            borderWidth,
            borderOutline,
            corner,
            titleRect,
            emulatorRect,
            hasHostTitle: true);
    }

    public static EmulatorGeometry CreateSnapshot(DeviceProfile device)
    {
        NFloat borderWidth = 8f;
        NFloat borderOutline = 2.5f;
        NFloat titleHeight = 0f;
        NFloat gap = 0f;
        NFloat corner = device.ScreenCornerRadius;
        NFloat margin = borderWidth;

        var emulatorRect = new Rect(
            margin,
            margin,
            device.LogicalResolution.Width,
            device.LogicalResolution.Height);
        var titleRect = new Rect(0, 0, 0, 0);

        return new EmulatorGeometry(
            titleHeight,
            gap,
            borderWidth,
            borderOutline,
            corner,
            titleRect,
            emulatorRect,
            hasHostTitle: false);
    }

    public Point MapEmulatorToHost(Point emulatorPoint) =>
        emulatorPoint + EmulatorRect.TopLeft;

    public Point MapHostToEmulator(Point hostPoint) =>
        new Point(
            hostPoint.X - EmulatorRect.X,
            hostPoint.Y - EmulatorRect.Y);

    public bool TryMapHostToEmulator(Point hostPoint, out Point emulatorPoint)
    {
        emulatorPoint = MapHostToEmulator(hostPoint);
        return EmulatorRect.Contains(hostPoint);
    }
}
