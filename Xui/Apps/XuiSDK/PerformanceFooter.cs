using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Apps.XuiSDK;

/// <summary>
/// An 18-pixel status bar drawn at the bottom of the window, showing live rendering
/// performance metrics: animation state, FPS, and per-frame heap allocation.
/// <para>
/// This view intentionally never calls <see cref="View.InvalidateRender"/>.
/// It is rendered whenever the rest of the window renders — driven by animations,
/// input, or other view state changes — and reads the <see cref="WindowRenderingMetrics"/>
/// snapshot that the window captures at the top of each frame.
/// </para>
/// </summary>
public class PerformanceFooter : View
{
    public static readonly nfloat Height = 18;

    // Fixed slot widths for the centred instrument group.
    // Sized to the widest text that can appear in each slot at font size 10.
    private const float StatusSlotWidth = 68;  // "Animated" is widest
    private const float FpsSlotWidth    = 48;  // "999 fps" is widest
    private const float AllocSlotWidth  = 148; // "Alloc: 9999.9 KB/frame" is widest
    private const float SlotGap         = 16;
    private const float GroupWidth      = StatusSlotWidth + SlotGap + FpsSlotWidth + SlotGap + AllocSlotWidth;

    private static readonly Color ColorText = new(0x00000099);

    private readonly WindowRenderingMetrics _metrics;

    public PerformanceFooter(WindowRenderingMetrics metrics)
    {
        _metrics = metrics;
    }

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        => new(availableSize.Width, Height);

    protected override void ArrangeCore(Rect rect, IMeasureContext context) { }

    protected override void RenderCore(IContext context)
    {
        var rect = Frame;
        var metrics = _metrics;

        context.SetFont(new Font(10, ["Segoe UI", "Inter", "sans-serif"]));
        context.TextBaseline = TextBaseline.Middle;
        context.TextAlign = TextAlign.Left;
        context.SetFill(ColorText);

        nfloat midY    = rect.Y + rect.Height / 2;
        nfloat groupX  = rect.X + (rect.Width - GroupWidth) / 2;

        // ── Slot 1: status ───────────────────────────────────────────────
        if (metrics.WasStalled)
            context.FillText($"Stalled", new Point(groupX, midY));
        else if (metrics.WasAnimated)
            context.FillText($"Animated", new Point(groupX, midY));
        else
            context.FillText($"Idle", new Point(groupX, midY));

        // ── Slot 2: FPS ───────────────────────────────────────────────────
        nfloat fpsX = groupX + StatusSlotWidth + SlotGap;
        if (metrics.SampleCount < 2)
            context.FillText($"-- fps", new Point(fpsX, midY));
        else
            context.FillText($"{(int)Math.Round(metrics.Fps)} fps", new Point(fpsX, midY));

        // ── Slot 3: alloc ─────────────────────────────────────────────────
        nfloat allocX = fpsX + FpsSlotWidth + SlotGap;
        long alloc = metrics.AllocBytesLastFrame;
        if (alloc >= 1_048_576)
            context.FillText($"Alloc: {alloc / 1_048_576.0:F1} MB/frame", new Point(allocX, midY));
        else if (alloc >= 1_024)
            context.FillText($"Alloc: {alloc / 1_024.0:F1} KB/frame", new Point(allocX, midY));
        else
            context.FillText($"Alloc: {alloc} Bytes/frame", new Point(allocX, midY));
    }
}
