using Xui.Core.Canvas;

namespace Xui.Core.Tests.Canvas;

public class FontMetricsCacheTests
{
    [Fact]
    public void Font_IsValueEqualWhenItsTypographyMatches()
    {
        var first = new Font(14, "Inter", FontWeight.SemiBold, FontStyle.Italic, FontStretch.Expanded);
        var second = new Font(14, "Inter", FontWeight.SemiBold, FontStyle.Italic, FontStretch.Expanded);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Cache_ReturnsMetricsForTheMatchingFont()
    {
        var cache = new FontMetricsCache();
        var font = new Font(16, "Inter");
        var metrics = new FontMetrics(12, 4, 11, 5, 0, -10, 4);

        cache.Set(font, metrics);

        Assert.True(cache.TryGet(font, out var actual));
        Assert.Equal(metrics.FontBoundingBoxAscent, actual.FontBoundingBoxAscent);
        Assert.Equal(metrics.IdeographicBaseline, actual.IdeographicBaseline);
    }

    [Fact]
    public void Cache_EvictsTheLeastRecentlyUsedFont()
    {
        var cache = new FontMetricsCache();
        var first = new Font(10, "First");
        var leastRecent = new Font(11, "Least recent");
        cache.Set(first, default);
        cache.Set(leastRecent, default);

        for (int i = 1; i < FontMetricsCache.Capacity - 1; i++)
            cache.Set(new Font(10 + i, $"Font {i}"), default);

        Assert.True(cache.TryGet(first, out _));

        cache.Set(new Font(100, "Newest"), default);

        Assert.True(cache.TryGet(first, out _));
        Assert.False(cache.TryGet(leastRecent, out _));
    }
}
