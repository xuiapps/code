namespace Xui.GPU.IR;

/// <summary>
/// Represents a return statement.
/// </summary>
public class IrReturn : IrStatement
{
    public override IrNodeKind Kind => IrNodeKind.Return;
    public IrExpression? Value { get; }

    public IrReturn(IrExpression? value = null)
    {
        Value = value;
    }
}
