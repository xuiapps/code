namespace Xui.GPU.IR;

/// <summary>
/// Represents a scalar type (F32, I32, U32, Bool).
/// </summary>
public class IrScalarType : IrType
{
    public override IrNodeKind Kind => IrNodeKind.ScalarType;
    public override string Name { get; }
    public ScalarKind ScalarKind { get; }

    public IrScalarType(ScalarKind kind)
    {
        ScalarKind = kind;
        Name = kind switch
        {
            ScalarKind.F32 => "F32",
            ScalarKind.I32 => "I32",
            ScalarKind.U32 => "U32",
            ScalarKind.Bool => "Bool",
            _ => throw new ArgumentException($"Unknown scalar kind: {kind}")
        };
    }
}
