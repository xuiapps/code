namespace Xui.Core.Curves2D;

/// <summary>
/// Holds one, two, or three Y-monotonic cubic Bézier segments that together represent
/// the result of splitting a single cubic at its Y-derivative roots.
/// </summary>
public readonly struct MonotonicCubicBezier
{
    /// <summary>The first (and always present) Y-monotonic segment.</summary>
    public readonly CubicBezier First;
    /// <summary>The second Y-monotonic segment, or <c>null</c> if the original had at most one Y-extremum.</summary>
    public readonly CubicBezier? Second;
    /// <summary>The third Y-monotonic segment, or <c>null</c> if the original had fewer than two Y-extrema.</summary>
    public readonly CubicBezier? Third;

    /// <summary>Initializes the struct with one, two, or three Y-monotonic segments.</summary>
    /// <param name="first">The first monotonic segment.</param>
    /// <param name="second">The second monotonic segment, if any.</param>
    /// <param name="third">The third monotonic segment, if any.</param>
    public MonotonicCubicBezier(CubicBezier first, CubicBezier? second = null, CubicBezier? third = null)
    {
        First = first;
        Second = second;
        Third = third;
    }
}
