namespace Xui.GPU.IR;

/// <summary>
/// Represents an interpolation mode decoration.
/// </summary>
public class IrInterpolationDecoration : IrDecoration
{
    /// <summary>Gets the IR node kind for this decoration.</summary>
    public override IrNodeKind Kind => IrNodeKind.Interpolation;
    /// <summary>Gets the interpolation mode.</summary>
    public InterpolationMode Mode { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrInterpolationDecoration"/> with the specified mode.
    /// </summary>
    public IrInterpolationDecoration(InterpolationMode mode)
    {
        Mode = mode;
    }
}
