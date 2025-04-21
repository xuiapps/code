namespace Xui.Core.Canvas;

/// <summary>
/// Specifies how two connected lines are joined when stroking a path.
/// Mirrors the <c>lineJoin</c> property in the HTML5 Canvas API.
/// </summary>
public enum LineJoin
{
    /// <summary>
    /// Lines are joined with a sharp corner or extended miter, depending on the miter limit.
    /// </summary>
    Miter = 0,

    /// <summary>
    /// Lines are joined with a circular arc, creating a rounded corner.
    /// </summary>
    Round = 1,

    /// <summary>
    /// Lines are joined with a beveled corner by connecting the outer corners of the strokes.
    /// </summary>
    Bevel = 2
}
