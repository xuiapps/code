using System.Diagnostics;
using Xui.Core.Canvas;
using Xui.Core.Debug;

namespace Xui.Core.UI;

/// <summary>
/// Reusable measurement-context decorator that records only time spent in the
/// underlying text-measure implementation.
/// </summary>
internal sealed class InstrumentedMeasureContext(InstrumentsAccessor instruments) : IMeasureContext
{
    private IMeasureContext inner = null!;

    public void SetInner(IMeasureContext value) => inner = value;

    public TextMetrics MeasureText(string text)
    {
        long start = Stopwatch.GetTimestamp();
        var metrics = inner.MeasureText(text);
        instruments.TrackTextMeasure(Stopwatch.GetTimestamp() - start);
        return metrics;
    }

    public TextMetrics MeasureText(ReadOnlySpan<char> text)
    {
        long start = Stopwatch.GetTimestamp();
        var metrics = inner.MeasureText(text);
        instruments.TrackTextMeasure(Stopwatch.GetTimestamp() - start);
        return metrics;
    }

    public void SetFont(Font font) => inner.SetFont(font);
}
