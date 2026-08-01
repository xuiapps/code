using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;

namespace Xui.Apps.TestApp.Pages.Gestures.Tests;

/// <summary>
/// A tiny draggable widget for the gesture demos. Captures the pointer as
/// <see cref="PointerGestures.Drag"/> on Down, so a parent ScrollView must
/// stand down and never steal — pointer drags become strokes drawn into the
/// pad's bounds. Strokes reset on each new press.
/// </summary>
public class ScratchPad : View
{
    private readonly List<Point> points = new(64);
    private bool drawing;

    public override int Count => 0;
    public override View this[int index] => throw new IndexOutOfRangeException();

    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        NFloat w = NFloat.IsFinite(available.Width)  ? NFloat.Min(available.Width,  320) : 320;
        NFloat h = NFloat.IsFinite(available.Height) ? NFloat.Min(available.Height, 160) : 160;
        return new Size(w, h);
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (phase != EventPhase.Tunnel) return;

        switch (e.Type)
        {
            case PointerEventType.Down:
                points.Clear();
                points.Add(ToLocal(e.State.Position));
                drawing = true;
                CapturePointer(e.PointerId, PointerGestures.Drag);
                InvalidateRender();
                break;

            case PointerEventType.Move when drawing:
                points.Add(ToLocal(e.State.Position));
                InvalidateRender();
                break;

            case PointerEventType.Up when drawing:
                drawing = false;
                ReleasePointer(e.PointerId);
                break;

            case PointerEventType.Cancel:
            case PointerEventType.LostCapture:
                drawing = false;
                break;
        }
    }

    // Translate from global (event-space) to local (Frame-relative) coordinates.
    // Stored strokes are kept in local space so they stay anchored to the pad as
    // the ScrollView arranges it at different Y positions.
    private Point ToLocal(Point global) => new Point(global.X - Frame.X, global.Y - Frame.Y);

    protected override void RenderCore(IContext context)
    {
        context.Save();

        // Background + border (frame is in global coords).
        context.BeginPath();
        context.RoundRect(Frame, 8);
        context.SetFill(new Color(0xF5, 0xF5, 0xF5, 0xFF));
        context.Fill(FillRule.NonZero);

        context.BeginPath();
        context.RoundRect(Frame, 8);
        context.SetStroke(new Color(0x99, 0x99, 0x99, 0xFF));
        context.LineWidth = 1;
        context.Stroke();

        // Clip strokes to bounds, then translate into local space so the stored
        // local-coordinate polyline renders relative to the pad's current Frame.
        context.BeginPath();
        context.RoundRect(Frame, 8);
        context.Clip();
        context.Translate(new Vector(Frame.X, Frame.Y));

        if (points.Count > 1)
        {
            context.SetStroke(new Color(0x1F, 0x6F, 0xEB, 0xFF));
            context.LineWidth = 3;
            context.BeginPath();
            context.MoveTo(points[0]);
            for (int i = 1; i < points.Count; i++)
                context.LineTo(points[i]);
            context.Stroke();
        }

        context.Restore();
    }
}
