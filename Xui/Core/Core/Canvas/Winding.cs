namespace Xui.Core.Canvas;

/// <summary>
/// Specifies the direction in which arcs or elliptical curves are drawn,
/// affecting fill and stroke behavior in path-based rendering.
/// </summary>
public enum Winding : int
{
    /// <summary>
    /// The path is drawn in a clockwise direction.
    /// This is the default in most canvas and geometry operations.
    /// </summary>
    ClockWise = 1,

    /// <summary>
    /// The path is drawn in a counter-clockwise direction.
    /// </summary>
    CounterClockWise = 0
}
