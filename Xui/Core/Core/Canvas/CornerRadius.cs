namespace Xui.Core.Canvas;

using System.Diagnostics;

/// <summary>
/// Represents the radius of each corner of a rectangle, allowing for uniform or non-uniform rounding.
/// </summary>
public struct CornerRadius
{
    /// <summary>
    /// Radius of the top-left corner.
    /// </summary>
    public nfloat TopLeft;

    /// <summary>
    /// Radius of the top-right corner.
    /// </summary>
    public nfloat TopRight;

    /// <summary>
    /// Radius of the bottom-right corner.
    /// </summary>
    public nfloat BottomRight;

    /// <summary>
    /// Radius of the bottom-left corner.
    /// </summary>
    public nfloat BottomLeft;

    /// <summary>
    /// Returns true if all corners have the same radius value.
    /// </summary>
    public bool IsUniform => this.TopLeft == this.TopRight && this.TopLeft == this.BottomRight && this.TopLeft == this.BottomLeft;

    /// <summary>
    /// Initializes a <see cref="CornerRadius"/> with the same radius applied to all four corners.
    /// </summary>
    /// <param name="radius">The uniform radius for all corners.</param>
    [DebuggerStepThrough]
    public CornerRadius(nfloat radius) : this()
    {
        this.TopLeft = this.TopRight = this.BottomRight = this.BottomLeft = radius;
    }

    /// <summary>
    /// Initializes a <see cref="CornerRadius"/> with individual values for each corner.
    /// </summary>
    /// <param name="topLeft">Radius of the top-left corner.</param>
    /// <param name="topRight">Radius of the top-right corner.</param>
    /// <param name="bottomRight">Radius of the bottom-right corner.</param>
    /// <param name="bottomLeft">Radius of the bottom-left corner.</param>
    [DebuggerStepThrough]
    public CornerRadius(nfloat topLeft, nfloat topRight, nfloat bottomRight, nfloat bottomLeft)
    {
        this.TopLeft = topLeft;
        this.TopRight = topRight;
        this.BottomRight = bottomRight;
        this.BottomLeft = bottomLeft;
    }

    /// <summary>
    /// Implicitly converts a single radius value to a uniform <see cref="CornerRadius"/>.
    /// </summary>
    /// <param name="radius">The uniform corner radius.</param>
    public static implicit operator CornerRadius(nfloat radius)
        => new CornerRadius(radius);

    /// <summary>
    /// Implicitly converts a single integer radius value to a uniform <see cref="CornerRadius"/>.
    /// </summary>
    /// <param name="radius">The uniform corner radius.</param>
    public static implicit operator CornerRadius(int radius)
        => new CornerRadius(radius);

    /// <summary>
    /// Implicitly converts a 4-tuple of radius values to a <see cref="CornerRadius"/>.
    /// </summary>
    /// <param name="radii">Tuple representing (TopLeft, TopRight, BottomRight, BottomLeft).</param>
    public static implicit operator CornerRadius(Tuple<nfloat, nfloat, nfloat, nfloat> radii) =>
        new CornerRadius(radii.Item1, radii.Item2, radii.Item3, radii.Item4);
}
