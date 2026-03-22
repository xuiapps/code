namespace Xui.GPU.IR;

/// <summary>
/// Represents a scalar type (F32, I32, U32, Bool).
/// </summary>
public class IrScalarType : IrType
{
    /// <summary>Gets the IR node kind for this type.</summary>
    public override IrNodeKind Kind => IrNodeKind.ScalarType;
    /// <summary>Gets the display name of this scalar type.</summary>
    public override string Name { get; }
    /// <summary>Gets the scalar kind.</summary>
    public ScalarKind ScalarKind { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrScalarType"/> with the specified scalar kind.
    /// </summary>
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
