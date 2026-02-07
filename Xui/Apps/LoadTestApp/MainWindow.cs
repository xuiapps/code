using System.Diagnostics;
using System.Runtime.InteropServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Window = Xui.Core.Abstract.Window;

namespace Xui.Apps.LoadTestApp;

public class MainWindow : Window
{
    private NFloat CellWidth = 80;
    private NFloat CellHeight = 22;
    private NFloat ScrollY = 0;

    // --- Perf sampling (rolling ~5 seconds) ---
    private const double SampleWindowSeconds = 5.0;

    private struct PerfSample
    {
        public long Timestamp;   // Stopwatch ticks
        public long AllocBytes;  // per-frame allocated bytes
    }

    private readonly Queue<PerfSample> samples = new(1024);

    private static double TicksToSeconds(long ticks)
        => (double)ticks / Stopwatch.Frequency;

    private void AddSample(long allocBytes)
    {
        long now = Stopwatch.GetTimestamp();

        this.samples.Enqueue(new PerfSample
        {
            Timestamp = now,
            AllocBytes = allocBytes
        });

        // Trim anything older than window
        long oldestAllowed = now - (long)(SampleWindowSeconds * Stopwatch.Frequency);
        while (this.samples.Count > 2 && this.samples.Peek().Timestamp < oldestAllowed)
        {
            this.samples.Dequeue();
        }
    }

    private void ComputeStats(out double fpsAvg, out long allocMin, out long allocMax, out long allocAvg, out int count)
    {
        count = this.samples.Count;

        if (count < 2)
        {
            fpsAvg = 0;
            allocMin = allocMax = allocAvg = 0;
            return;
        }

        // Iterate without allocations
        long firstTs = 0;
        long lastTs = 0;

        long min = long.MaxValue;
        long max = long.MinValue;
        long sum = 0;

        bool first = true;
        foreach (var s in this.samples)
        {
            if (first)
            {
                firstTs = s.Timestamp;
                first = false;
            }

            lastTs = s.Timestamp;

            long a = s.AllocBytes;
            if (a < min) min = a;
            if (a > max) max = a;
            sum += a;
        }

        double dt = TicksToSeconds(lastTs - firstTs);
        fpsAvg = dt > 0 ? (count - 1) / dt : 0;

        allocMin = min == long.MaxValue ? 0 : min;
        allocMax = max == long.MinValue ? 0 : max;
        allocAvg = sum / count;
    }

    public override void Render(ref RenderEventRef render)
    {
        long allocBefore = GC.GetAllocatedBytesForCurrentThread();

        using var ctx = Xui.Core.Actual.Runtime.DrawingContext!;

        // Clear background
        ctx.SetFill(Colors.Black);
        ctx.FillRect(render.Rect);

        // Infinite vertical scroll
        this.ScrollY += 2;

        int startRow = (int)Math.Floor((double)(this.ScrollY / this.CellHeight));
        int endRow = (int)Math.Ceiling((double)((this.ScrollY + render.Rect.Height) / this.CellHeight));
        int startCol = 0;
        int endCol = (int)Math.Ceiling((double)(render.Rect.Width / this.CellWidth));

        ctx.SetFont(new Font
        {
            FontFamily = ["Inter", "sans-serif"],
            FontSize = 11,
            FontWeight = 400
        });
        ctx.TextAlign = TextAlign.Center;
        ctx.TextBaseline = TextBaseline.Middle;

        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                NFloat x = col * this.CellWidth;
                NFloat y = row * this.CellHeight - this.ScrollY;

                NFloat r = (NFloat)(0.5 + 0.5 * Math.Sin(col * 0.15 + row * 0.02));
                NFloat g = (NFloat)(0.5 + 0.5 * Math.Sin(col * 0.1 + row * 0.03 + 2));
                NFloat b = (NFloat)(0.5 + 0.5 * Math.Sin(col * 0.12 + row * 0.025 + 4));

                var cellRect = new Rect(x, y, this.CellWidth - 1, this.CellHeight - 1);

                ctx.SetFill(new Color(r, g, b, 1));
                ctx.FillRect(cellRect);

                NFloat brightness = (r + g + b) / 3;
                ctx.SetFill(brightness > 0.5 ? Colors.Black : Colors.White);
                ctx.FillText($"{col},{row}", (x + this.CellWidth / 2, y + this.CellHeight / 2));
            }
        }

        // --- Perf ---
        long allocAfter = GC.GetAllocatedBytesForCurrentThread();
        long allocated = allocAfter - allocBefore;

        this.AddSample(allocated);

        this.ComputeStats(
            out double fpsAvg,
            out long allocMin,
            out long allocMax,
            out long allocAvg,
            out int sampleCount);

        long allocRange = allocMax - allocMin;

        // Overlay (one metric per line, stable-ish formatting)
        ctx.SetFont(new Font
        {
            FontFamily = ["Inter", "sans-serif"],
            FontSize = 14,
            FontWeight = 700
        });
        ctx.TextAlign = TextAlign.Left;
        ctx.TextBaseline = TextBaseline.Top;

        // Round FPS to an integer so it doesn't wobble 59.9 / 60.9
        int fpsRounded = (int)Math.Round(fpsAvg);

        // Optional: also round to nearest 10 bytes to reduce tiny jitter
        static long RoundTo(long value, long step)
            => step <= 1 ? value : ((value + (step / 2)) / step) * step;

        long allocNowR   = RoundTo(allocated, 10);
        long allocAvgR   = RoundTo(allocAvg, 10);
        long allocMinR   = RoundTo(allocMin, 10);
        long allocMaxR   = RoundTo(allocMax, 10);
        long allocRangeR = RoundTo(allocRange, 10);

        const int lines = 4;
        const int lineH = 18;
        int overlayH = 12 + lines * lineH + 6;

        ctx.SetFill(new Color(0, 0, 0, 0.7f));
        ctx.FillRect(new Rect(8, 8, 360, overlayH));

        ctx.SetFill(Colors.Lime);

        int y2 = 12;
        ctx.FillText($"FPS(avg {SampleWindowSeconds:0}s): {fpsRounded}", (12, y2));
        y2 += lineH;
        ctx.FillText($"Samples: {sampleCount}", (12, y2));
        y2 += lineH;
        ctx.FillText($"Alloc Now: {allocNowR} B", (12, y2));
        y2 += lineH;
        ctx.FillText($"Alloc Range: {allocRangeR}", (12, y2));

        // Request continuous animation
        Invalidate();

        base.Render(ref render);
    }
}
