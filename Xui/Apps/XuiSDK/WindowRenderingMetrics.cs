using System.Diagnostics;

namespace Xui.Apps.XuiSDK;

/// <summary>
/// Tracks per-frame rendering performance metrics for a window.
/// <para>
/// Updated by <see cref="Window"/> at the start and end of each render pass.
/// Read by diagnostic UI (e.g. a performance footer) during <c>RenderCore</c>.
/// The footer never needs to call <c>InvalidateRender</c> — it is rendered
/// whenever the rest of the window renders, and reads the metrics captured
/// at the top of that same frame.
/// </para>
/// </summary>
public sealed class WindowRenderingMetrics
{
    // Half-second rolling window at up to 120 fps — 60 slots is plenty.
    private const int MaxSamples = 60;
    private static readonly long HalfSecondTicks = Stopwatch.Frequency / 2;
    private static readonly long StallTicks = Stopwatch.Frequency / 4; // 250 ms

    private struct Sample
    {
        // Stopwatch ticks at BeginFrame
        public long Timestamp;
    }

    private readonly Sample[] _samples = new Sample[MaxSamples];
    private int _index;
    private int _count;

    private long _prevAllocBytes;
    private bool _hasPrevAlloc;

    private long _frameStartTick;
    private long _prevFrameStartTick;
    private bool _hasPrevFrameStart;

    /// <summary>Frames per second averaged over the last 0.5-second sample window.</summary>
    public double Fps { get; private set; }

    /// <summary>Bytes allocated on the managed heap during the most recent frame.</summary>
    public long AllocBytesLastFrame { get; private set; }

    /// <summary>
    /// Whether at least one view had an animation callback scheduled this frame
    /// (i.e. <see cref="Xui.Core.UI.View.ViewFlags.Animated"/> or
    /// <see cref="Xui.Core.UI.View.ViewFlags.DescendantAnimated"/> was set on
    /// the root view when <c>Render</c> began).
    /// </summary>
    public bool WasAnimated { get; private set; }

    /// <summary>
    /// Whether the gap between this frame and the previous one exceeded 250 ms,
    /// indicating the UI thread stalled (GC pause, long layout pass, etc.).
    /// </summary>
    public bool WasStalled { get; private set; }

    /// <summary>Number of samples currently in the rolling window.</summary>
    public int SampleCount => _count;

    /// <summary>
    /// Called at the very start of <see cref="Window.Render"/> — before the layout/render pass —
    /// to snapshot pre-frame state.
    /// </summary>
    /// <param name="animated">
    /// Whether any view had animation callbacks registered this frame.
    /// Callers should check <c>RootView.Flags</c> for
    /// <c>Animated | DescendantAnimated</c> before invoking base.Render.
    /// </param>
    public void BeginFrame(bool animated)
    {
        _frameStartTick = Stopwatch.GetTimestamp();
        WasAnimated = animated;

        // Stall detection: gap since last frame start.
        if (_hasPrevFrameStart)
            WasStalled = (_frameStartTick - _prevFrameStartTick) > StallTicks;
        else
            WasStalled = false;

        _prevFrameStartTick = _frameStartTick;
        _hasPrevFrameStart = true;

        // Allocation diff vs previous frame.
        long memNow = GC.GetAllocatedBytesForCurrentThread();
        if (_hasPrevAlloc)
        {
            long delta = memNow - _prevAllocBytes;
            AllocBytesLastFrame = delta >= 0 ? delta : 0;
        }
        else
        {
            AllocBytesLastFrame = 0;
        }
        _prevAllocBytes = memNow;
        _hasPrevAlloc = true;
    }

    /// <summary>
    /// Called at the very end of <see cref="Window.Render"/> — after the layout/render pass —
    /// to commit the sample and recompute FPS.
    /// </summary>
    public void EndFrame()
    {
        _samples[_index] = new Sample { Timestamp = _frameStartTick };
        _index = (_index + 1) % MaxSamples;
        if (_count < MaxSamples) _count++;

        ComputeFps();
    }

    private void ComputeFps()
    {
        if (_count < 2)
        {
            Fps = 0;
            return;
        }

        // Walk backwards from the newest sample and keep only those within 0.5 s.
        int newestIdx = (_index - 1 + MaxSamples) % MaxSamples;
        long cutoff = _samples[newestIdx].Timestamp - HalfSecondTicks;

        int used = 0;
        for (int i = 0; i < _count; i++)
        {
            int idx = (_index - 1 - i + MaxSamples * 2) % MaxSamples;
            if (_samples[idx].Timestamp >= cutoff)
                used++;
            else
                break;
        }

        if (used < 2)
        {
            Fps = 0;
            return;
        }

        int oldestIdx = (_index - used + MaxSamples * 2) % MaxSamples;
        double dt = (double)(_samples[newestIdx].Timestamp - _samples[oldestIdx].Timestamp)
                    / Stopwatch.Frequency;
        Fps = dt > 0 ? (used - 1) / dt : 0;
    }
}
