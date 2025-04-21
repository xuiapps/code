namespace Xui.Core.Math2D;

/// <summary>
/// Represents thickness values for each edge of a rectangular element.
/// </summary>
/// <remarks>
/// A <see cref="Frame"/> defines spacing from the left, top, right, and bottom edges.
/// It is typically used for layout constructs such as <c>Margin</c>, <c>Padding</c>, or <c>BorderWidth</c>.
/// Unlike <see cref="Rect"/>, it does not represent position or size—only edge offsets.
/// </remarks>
public struct Frame
{
    /// <summary>The thickness of the top edge.</summary>
    public nfloat Top;

    /// <summary>The thickness of the right edge.</summary>
    public nfloat Right;

    /// <summary>The thickness of the bottom edge.</summary>
    public nfloat Bottom;

    /// <summary>The thickness of the left edge.</summary>
    public nfloat Left;

    /// <summary>
    /// Returns <c>true</c> if all four edges have equal thickness.
    /// </summary>
    public bool IsUniform => Left == Top && Left == Right && Left == Bottom;

    /// <summary>
    /// Gets the total horizontal edge thickness (left + right).
    /// </summary>
    public nfloat TotalWidth => Left + Right;

    /// <summary>
    /// Gets the total vertical edge thickness (top + bottom).
    /// </summary>
    public nfloat TotalHeight => Top + Bottom;

    /// <summary>
    /// A frame with all edge values set to zero.
    /// </summary>
    public static readonly Frame Zero = new Frame();

    /// <summary>
    /// Initializes a new <see cref="Frame"/> with all edges set to zero.
    /// </summary>
    [DebuggerStepThrough]
    public Frame()
    {
        Left = Top = Right = Bottom = 0;
    }

    /// <summary>
    /// Initializes a new <see cref="Frame"/> with the specified edge thicknesses.
    /// </summary>
    /// <param name="top">Top edge thickness.</param>
    /// <param name="right">Right edge thickness.</param>
    /// <param name="bottom">Bottom edge thickness.</param>
    /// <param name="left">Left edge thickness.</param>
    [DebuggerStepThrough]
    public Frame(nfloat top, nfloat right, nfloat bottom, nfloat left)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }

    /// <summary>
    /// Converts a 4-tuple (top, right, bottom, left) to a <see cref="Frame"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Frame((nfloat top, nfloat right, nfloat bottom, nfloat left) value) =>
        new Frame(value.top, value.right, value.bottom, value.left);

    /// <summary>
    /// Converts a 2-tuple (horizontal, vertical) to a <see cref="Frame"/>.
    /// Horizontal is applied to left and right; vertical to top and bottom.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Frame((nfloat horizontal, nfloat vertical) value) =>
        new Frame(value.vertical, value.horizontal, value.vertical, value.horizontal);

    /// <summary>
    /// Converts a scalar value into a uniform <see cref="Frame"/> for all sides.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Frame(nfloat value) =>
        new Frame(value, value, value, value);

    /// <summary>
    /// Implicitly converts an <see cref="int"/> to a <see cref="Frame"/>,
    /// applying the same value to all four sides (Left, Top, Right, Bottom).
    /// </summary>
    /// <param name="value">The integer value to apply uniformly to all sides.</param>
    /// <returns>A <see cref="Frame"/> with all sides set to <paramref name="value"/>.</returns>
    [DebuggerStepThrough]
    public static implicit operator Frame(int value) =>
        new Frame(value, value, value, value);

    /// <summary>
    /// Implicitly converts a <see cref="double"/> to a <see cref="Frame"/>,
    /// applying the same value to all four sides (Left, Top, Right, Bottom).
    /// </summary>
    /// <param name="value">The double value to apply uniformly to all sides.</param>
    /// <returns>A <see cref="Frame"/> with all sides set to <paramref name="value"/>.</returns>
    [DebuggerStepThrough]
    public static implicit operator Frame(double value) =>
        new Frame((nfloat)value, (nfloat)value, (nfloat)value, (nfloat)value);

    /// <summary>
    /// Implicitly converts a <see cref="float"/> to a <see cref="Frame"/>,
    /// applying the same value to all four sides (Left, Top, Right, Bottom).
    /// </summary>
    /// <param name="value">The float value to apply uniformly to all sides.</param>
    /// <returns>A <see cref="Frame"/> with all sides set to <paramref name="value"/>.</returns>
    [DebuggerStepThrough]
    public static implicit operator Frame(float value) =>
        new Frame((nfloat)value, (nfloat)value, (nfloat)value, (nfloat)value);

    /// <summary>
    /// Scales all edges of the frame by the specified scalar.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame operator *(Frame lhs, nfloat rhs) =>
        new Frame(lhs.Top * rhs, lhs.Right * rhs, lhs.Bottom * rhs, lhs.Left * rhs);

    /// <summary>
    /// Adds the corresponding edge thicknesses of two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame operator +(Frame lhs, Frame rhs) =>
        new Frame(lhs.Top + rhs.Top, lhs.Right + rhs.Right, lhs.Bottom + rhs.Bottom, lhs.Left + rhs.Left);

    /// <summary>
    /// Subtracts the corresponding edge thicknesses of two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame operator -(Frame lhs, Frame rhs) =>
        new Frame(lhs.Top - rhs.Top, lhs.Right - rhs.Right, lhs.Bottom - rhs.Bottom, lhs.Left - rhs.Left);

    /// <summary>
    /// Returns a frame containing the maximum value for each edge from two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame Max(Frame lhs, Frame rhs) => (
        nfloat.Max(lhs.Top, rhs.Top),
        nfloat.Max(lhs.Right, rhs.Right),
        nfloat.Max(lhs.Bottom, rhs.Bottom),
        nfloat.Max(lhs.Left, rhs.Left)
    );

    /// <summary>
    /// Returns a frame containing the minimum value for each edge from two frames.
    /// </summary>
    [DebuggerStepThrough]
    public static Frame Min(Frame lhs, Frame rhs) => (
        nfloat.Min(lhs.Top, rhs.Top),
        nfloat.Min(lhs.Right, rhs.Right),
        nfloat.Min(lhs.Bottom, rhs.Bottom),
        nfloat.Min(lhs.Left, rhs.Left)
    );

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString() =>
        $"Frame [Top={Top}, Right={Right}, Bottom={Bottom}, Left={Left}]";

    /// <summary>
    /// Returns <c>true</c> if all edge values match exactly.
    /// </summary>
    public static bool operator ==(Frame left, Frame right) =>
        left.Top == right.Top &&
        left.Right == right.Right &&
        left.Bottom == right.Bottom &&
        left.Left == right.Left;

    /// <summary>
    /// Returns <c>true</c> if any edge value differs.
    /// </summary>
    public static bool operator !=(Frame lhs, Frame rhs) =>
        !(lhs == rhs);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Frame other && this == other;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(Top, Right, Bottom, Left);
}
