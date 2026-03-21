namespace Xui.GPU.IR;

/// <summary>
/// Represents an interpolation mode decoration.
/// </summary>
public class IrInterpolationDecoration : IrDecoration
{
    public override IrNodeKind Kind => IrNodeKind.Interpolation;
    public InterpolationMode Mode { get; }

    public IrInterpolationDecoration(InterpolationMode mode)
    {
        Mode = mode;
    }
}
