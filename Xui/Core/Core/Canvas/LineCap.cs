namespace Xui.Core.Canvas;

/// <summary>
/// Specifies the shape used at the ends of lines when stroking paths.
/// Mirrors the <c>lineCap</c> property in the HTML5 Canvas API.
/// </summary>
public enum LineCap
{
    /// <summary>
    /// The line ends exactly at the endpoint with no additional extension. This is the default.
    /// </summary>
    Butt = 0,

    /// <summary>
    /// The line ends with a semicircular extension, centered on the endpoint.
    /// </summary>
    Round = 1,

    /// <summary>
    /// The line ends with a square extension equal to half the line width, extending beyond the endpoint.
    /// </summary>
    Square = 2
}
