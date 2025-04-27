namespace Xui.Core.Math2D;

/// <summary>
/// Represents a two-dimensional size with <see cref="Width"/> and <see cref="Height"/> components.
/// </summary>
/// <remarks>
/// A <see cref="Size"/> specifies how much space an element occupies, without defining its position.
/// It is commonly used in layout systems to describe constraints or measured results.
/// </remarks>
public struct Size
{
    /// <summary>The width of the element.</summary>
    public nfloat Width;

    /// <summary>The height of the element.</summary>
    public nfloat Height;

    /// <summary>A size with zero width and height.</summary>
    public static readonly Size Empty = new Size(0, 0);

    /// <summary>A size with infinite width and height.</summary>
    public static readonly Size Infinity = new Size(nfloat.PositiveInfinity, nfloat.PositiveInfinity);

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> struct with zero dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public Size()
    {
        this.Width = 0;
        this.Height = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> struct with the given width and height.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    [DebuggerStepThrough]
    public Size(nfloat width, nfloat height)
    {
        this.Width = width;
        this.Height = height;
    }

    /// <summary>
    /// Implicitly converts a tuple to a <see cref="Size"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Size(ValueTuple<nfloat, nfloat> tuple) =>
        new(tuple.Item1, tuple.Item2);
    
    /// <summary>
    /// Returns the size of a square.
    /// </summary>
    public static implicit operator Size(int uniform) =>
        new (uniform, uniform);

    /// <summary>
    /// Explicitly converts a <see cref="Vector"/> to a <see cref="Size"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static explicit operator Size(Vector vector) =>
        new(vector.X, vector.Y);

    /// <summary>
    /// Adds edge spacing from a <see cref="Frame"/> to a <see cref="Size"/>, increasing its dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static Size operator +(Size size, Frame frame) =>
        (size.Width + frame.Left + frame.Right, size.Height + frame.Top + frame.Bottom);

    /// <summary>
    /// Subtracts edge spacing from a <see cref="Size"/>, reducing its dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static Size operator -(Size size, Frame frame) =>
        (size.Width - frame.Left - frame.Right, size.Height - frame.Top - frame.Bottom);

    /// <summary>
    /// Adds two sizes together component-wise.
    /// </summary>
    [DebuggerStepThrough]
    public static Size operator +(Size lhs, Size rhs) =>
        (lhs.Width + rhs.Width, lhs.Height + rhs.Height);

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString() =>
        $"Size [Width={this.Width}, Height={this.Height}]";

    /// <summary>
    /// Returns <c>true</c> if the two sizes have the same dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator ==(Size lhs, Size rhs) =>
        lhs.Width == rhs.Width && lhs.Height == rhs.Height;

    /// <summary>
    /// Returns <c>true</c> if the two sizes have different dimensions.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator !=(Size lhs, Size rhs) =>
        lhs.Width != rhs.Width || lhs.Height != rhs.Height;

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override bool Equals(object? obj) =>
        obj is Size other && this == other;

    /// <summary>
    /// Returns <c>true</c> if this size is equal to the specified <paramref name="other"/>.
    /// </summary>
    public bool Equals(Size other) => this == other;

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override int GetHashCode() =>
        HashCode.Combine(Width, Height);

    /// <summary>
    /// Returns the minimum of two sizes, using the smaller width and height from each.
    /// </summary>
    public static Size Min(Size lhs, Size rhs) => (
        nfloat.Min(lhs.Width, rhs.Width),
        nfloat.Min(lhs.Height, rhs.Height)
    );

    /// <summary>
    /// Returns the maximum of two sizes, using the larger width and height from each.
    /// </summary>
    public static Size Max(Size lhs, Size rhs) => (
        nfloat.Max(lhs.Width, rhs.Width),
        nfloat.Max(lhs.Height, rhs.Height)
    );
}
