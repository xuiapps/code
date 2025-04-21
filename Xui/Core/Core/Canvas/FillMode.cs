namespace Xui.Core.Canvas;

/// <summary>
/// Specifies the algorithm used to determine the "insideness" of a point in a path,
/// controlling how filled shapes are rendered.
/// </summary>
public enum FillRule
{
    /// <summary>
    /// Uses the non-zero winding rule. A point is inside the path if the sum of
    /// path segment windings around it is non-zero. This is the default rule.
    /// </summary>
    NonZero,

    /// <summary>
    /// Uses the even-odd rule. A point is inside the path if a ray drawn from it
    /// to infinity crosses the path segments an odd number of times.
    /// </summary>
    EvenOdd
}
