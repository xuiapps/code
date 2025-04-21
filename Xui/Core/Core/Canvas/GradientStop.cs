namespace Xui.Core.Canvas;

/// <summary>
/// Represents a single color stop in a gradient, defined by an offset and a color.
/// Used for both linear and radial gradients.
/// </summary>
public struct GradientStop
{
    /// <summary>
    /// The position of the stop within the gradient, typically in the range [0, 1].
    /// </summary>
    public nfloat Offset;

    /// <summary>
    /// The color at the specified offset.
    /// </summary>
    public Color Color;

    /// <summary>
    /// Initializes a new <see cref="GradientStop"/> with a specified offset and color.
    /// </summary>
    /// <param name="offset">Position of the color stop, usually from 0 to 1.</param>
    /// <param name="color">Color at the given offset.</param>
    public GradientStop(nfloat offset, Color color)
    {
        this.Offset = offset;
        this.Color = color;
    }

    /// <summary>
    /// Initializes a new <see cref="GradientStop"/> with a specified offset and packed RGBA color.
    /// </summary>
    /// <param name="offset">Position of the color stop, usually from 0 to 1.</param>
    /// <param name="color">Color in packed 0xRRGGBBAA format.</param>
    public GradientStop(nfloat offset, uint color)
    {
        this.Offset = offset;
        this.Color = new Color(color);
    }

    /// <summary>
    /// Implicitly converts a tuple of <see cref="nfloat"/> and <see cref="Color"/> to a <see cref="GradientStop"/>.
    /// </summary>
    /// <param name="pair">A tuple containing offset and color.</param>
    public static implicit operator GradientStop((nfloat Offset, Color Color) pair) =>
        new GradientStop(pair.Offset, pair.Color);

    /// <summary>
    /// Implicitly converts a tuple of <see cref="nfloat"/> and <c>uint</c> (0xRRGGBBAA) to a <see cref="GradientStop"/>.
    /// </summary>
    /// <param name="pair">A tuple containing offset and packed RGBA color.</param>
    public static implicit operator GradientStop((nfloat Offset, uint Color) pair) =>
        new GradientStop(pair.Offset, pair.Color);
}
