namespace Xui.Core.Curves2D;

/// <summary>
/// Holds one or two Y-monotonic quadratic Bézier segments that together represent
/// a single quadratic curve split at its Y-extremum.
/// </summary>
public readonly struct MonotonicQuadraticBezier
{
    /// <summary>The first (and always present) Y-monotonic segment.</summary>
    public readonly QuadraticBezier First;
    /// <summary>The second Y-monotonic segment, or <c>null</c> if no split was needed.</summary>
    public readonly QuadraticBezier? Second;

    /// <summary>Initializes the struct with one or two Y-monotonic segments.</summary>
    /// <param name="first">The first monotonic segment.</param>
    /// <param name="second">The second monotonic segment, if any.</param>
    public MonotonicQuadraticBezier(QuadraticBezier first, QuadraticBezier? second = null)
    {
        First = first;
        Second = second;
    }
}
