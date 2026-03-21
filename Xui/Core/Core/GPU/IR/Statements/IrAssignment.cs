namespace Xui.GPU.IR;

/// <summary>
/// Represents an assignment statement.
/// </summary>
public class IrAssignment : IrStatement
{
    public override IrNodeKind Kind => IrNodeKind.Assignment;
    public IrExpression Target { get; }
    public IrExpression Value { get; }

    public IrAssignment(IrExpression target, IrExpression value)
    {
        Target = target;
        Value = value;
    }
}
