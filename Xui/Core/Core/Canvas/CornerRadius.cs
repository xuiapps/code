namespace Xui.Core.Canvas;

using System.Diagnostics;

/// <summary>
/// Represents the radius of each corner of a rectangle, allowing for uniform or non-uniform rounding.
/// </summary>
public struct CornerRadius
{
    /// <summary>
    /// A <see cref="CornerRadius"/> where all corners have a radius of zero.
    /// </summary>
    public static readonly CornerRadius Zero = new CornerRadius();

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
    /// Returns true if all corner radii are zero.
    /// </summary>
    public readonly bool IsZero => this.TopLeft == 0 && this.TopRight == 0 && this.BottomRight == 0 && this.BottomLeft == 0;

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

    /// <summary>
    /// Adds two <see cref="CornerRadius"/> values component-wise.
    /// </summary>
    /// <param name="lhs">The first <see cref="CornerRadius"/>.</param>
    /// <param name="rhs">The second <see cref="CornerRadius"/>.</param>
    /// <returns>A new <see cref="CornerRadius"/> where each corner is the sum of the corresponding corners.</returns>
    [DebuggerStepThrough]
    public static CornerRadius operator +(CornerRadius lhs, CornerRadius rhs)
    {
        return new CornerRadius(
            lhs.TopLeft + rhs.TopLeft,
            lhs.TopRight + rhs.TopRight,
            lhs.BottomRight + rhs.BottomRight,
            lhs.BottomLeft + rhs.BottomLeft
        );
    }

    /// <summary>
    /// Subtracts one <see cref="CornerRadius"/> from another component-wise.
    /// </summary>
    /// <param name="lhs">The first <see cref="CornerRadius"/>.</param>
    /// <param name="rhs">The second <see cref="CornerRadius"/> to subtract.</param>
    /// <returns>A new <see cref="CornerRadius"/> where each corner is the difference of the corresponding corners.</returns>
    [DebuggerStepThrough]
    public static CornerRadius operator -(CornerRadius lhs, CornerRadius rhs)
    {
        return new CornerRadius(
            lhs.TopLeft - rhs.TopLeft,
            lhs.TopRight - rhs.TopRight,
            lhs.BottomRight - rhs.BottomRight,
            lhs.BottomLeft - rhs.BottomLeft
        );
    }

    /// <summary>
    /// Returns a <see cref="CornerRadius"/> where each corner is the minimum of the corresponding corners of the two inputs.
    /// </summary>
    /// <param name="a">First <see cref="CornerRadius"/>.</param>
    /// <param name="b">Second <see cref="CornerRadius"/>.</param>
    /// <returns>A new <see cref="CornerRadius"/> taking the minimum value at each corner.</returns>
    [DebuggerStepThrough]
    public static CornerRadius Min(CornerRadius a, CornerRadius b)
    {
        return new CornerRadius(
            nfloat.Min(a.TopLeft, b.TopLeft),
            nfloat.Min(a.TopRight, b.TopRight),
            nfloat.Min(a.BottomRight, b.BottomRight),
            nfloat.Min(a.BottomLeft, b.BottomLeft)
        );
    }

    /// <summary>
    /// Returns a <see cref="CornerRadius"/> where each corner is the maximum of the corresponding corners of the two inputs.
    /// </summary>
    /// <param name="a">First <see cref="CornerRadius"/>.</param>
    /// <param name="b">Second <see cref="CornerRadius"/>.</param>
    /// <returns>A new <see cref="CornerRadius"/> taking the maximum value at each corner.</returns>
    [DebuggerStepThrough]
    public static CornerRadius Max(CornerRadius a, CornerRadius b)
    {
        return new CornerRadius(
            nfloat.Max(a.TopLeft, b.TopLeft),
            nfloat.Max(a.TopRight, b.TopRight),
            nfloat.Max(a.BottomRight, b.BottomRight),
            nfloat.Max(a.BottomLeft, b.BottomLeft)
        );
    }

    /// <summary>
    /// Implicitly converts a tuple (horizontal, vertical) into a <see cref="CornerRadius"/>,
    /// where TopLeft and BottomRight use horizontal, and TopRight and BottomLeft use vertical radius.
    /// </summary>
    /// <param name="radii">Tuple of two radii (horizontal, vertical).</param>
    [DebuggerStepThrough]
    public static implicit operator CornerRadius((nfloat horizontal, nfloat vertical) radii)
    {
        return new CornerRadius(
            radii.horizontal,
            radii.vertical,
            radii.horizontal,
            radii.vertical
        );
    }

    /// <summary>
    /// Implicitly converts a tuple (topLeft, topRight, bottomRight, bottomLeft) into a <see cref="CornerRadius"/>.
    /// </summary>
    /// <param name="radii">Tuple of four radii representing each corner individually.</param>
    [DebuggerStepThrough]
    public static implicit operator CornerRadius((nfloat topLeft, nfloat topRight, nfloat bottomRight, nfloat bottomLeft) radii)
    {
        return new CornerRadius(
            radii.topLeft,
            radii.topRight,
            radii.bottomRight,
            radii.bottomLeft
        );
    }
}
