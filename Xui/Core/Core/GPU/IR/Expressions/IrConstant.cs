namespace Xui.GPU.IR;

/// <summary>
/// Represents a constant value.
/// </summary>
public class IrConstant : IrExpression
{
    /// <summary>Gets the node kind for this constant.</summary>
    public override IrNodeKind Kind => IrNodeKind.Constant;

    /// <summary>Gets the type of the constant.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the constant value.</summary>
    public object Value { get; }

    /// <summary>Initializes a new instance of the <see cref="IrConstant"/> class.</summary>
    public IrConstant(IrType type, object value)
    {
        Type = type;
        Value = value;
    }
}
