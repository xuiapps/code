namespace Xui.Core.Canvas;

/// <summary>
/// Fixed-capacity, allocation-free-after-construction LRU cache of font-wide metrics.
/// One instance belongs to a drawing or measurement context because native font resolution is
/// surface-specific.
/// </summary>
public sealed class FontMetricsCache : FontCache<FontMetrics>
{
    /// <summary>Maximum number of distinct font descriptions retained by this cache.</summary>
    public const int Capacity = DefaultCapacity;

    /// <summary>Initializes the standard 64-entry metrics cache.</summary>
    public FontMetricsCache() : base(Capacity) { }
}
