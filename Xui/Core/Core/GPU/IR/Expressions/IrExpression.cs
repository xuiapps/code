namespace Xui.GPU.IR;

/// <summary>
/// Base class for all expression nodes in the IR.
/// </summary>
public abstract class IrExpression : IrNode
{
    /// <summary>
    /// Gets the type of this expression.
    /// </summary>
    public abstract IrType Type { get; }
}
