namespace Xui.Core.Curves2D;

/// <summary>
/// Represents one or two <see cref="Arc3Point"/> segments that are each monotonic in the Y direction.
/// </summary>
public readonly struct MonotonicArc3Point
{
    /// <summary>The first monotonic segment.</summary>
    public readonly Arc3Point First;

    /// <summary>The optional second segment, if the arc was split.</summary>
    public readonly Arc3Point? Second;

    /// <summary>Creates a single-segment monotonic arc.</summary>
    public MonotonicArc3Point(Arc3Point only)
    {
        First = only;
        Second = null;
    }

    /// <summary>Creates a two-segment Y-monotonic arc.</summary>
    public MonotonicArc3Point(Arc3Point first, Arc3Point second)
    {
        First = first;
        Second = second;
    }

    /// <summary>True if the arc was split into two segments.</summary>
    public bool IsSplit => Second.HasValue;
}
