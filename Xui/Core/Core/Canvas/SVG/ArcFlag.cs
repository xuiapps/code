/// <summary>
/// Specifies the size of the arc to be drawn when interpreting an SVG arc command.
/// Used in conjunction with the sweep flag to determine the final arc segment.
///
/// This corresponds to the "large-arc-flag" in the SVG path specification.
/// </summary>
public enum ArcFlag
{
    /// <summary>
    /// Draw the smaller of the two possible arc sweeps (less than or equal to 180 degrees).
    /// </summary>
    Small = 0,

    /// <summary>
    /// Draw the larger of the two possible arc sweeps (greater than 180 degrees).
    /// </summary>
    Large = 1
}