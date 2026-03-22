namespace Xui.GPU.IR;

/// <summary>
/// Represents an assignment statement.
/// </summary>
public class IrAssignment : IrStatement
{
    /// <summary>Gets the IR node kind for this statement.</summary>
    public override IrNodeKind Kind => IrNodeKind.Assignment;
    /// <summary>Gets the assignment target expression.</summary>
    public IrExpression Target { get; }
    /// <summary>Gets the value expression to assign.</summary>
    public IrExpression Value { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrAssignment"/> with the specified target and value.
    /// </summary>
    public IrAssignment(IrExpression target, IrExpression value)
    {
        Target = target;
        Value = value;
    }
}
