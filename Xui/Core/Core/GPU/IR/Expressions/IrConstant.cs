namespace Xui.GPU.IR;

/// <summary>
/// Represents a constant value.
/// </summary>
public class IrConstant : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Constant;
    public override IrType Type { get; }
    public object Value { get; }

    public IrConstant(IrType type, object value)
    {
        Type = type;
        Value = value;
    }
}
