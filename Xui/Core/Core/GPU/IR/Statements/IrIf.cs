namespace Xui.GPU.IR;

/// <summary>
/// Represents an if statement.
/// </summary>
public class IrIf : IrStatement
{
    /// <summary>Gets the IR node kind for this statement.</summary>
    public override IrNodeKind Kind => IrNodeKind.If;
    /// <summary>Gets the condition expression.</summary>
    public IrExpression Condition { get; }
    /// <summary>Gets the block executed when the condition is true.</summary>
    public IrBlock ThenBlock { get; }
    /// <summary>Gets the optional block executed when the condition is false.</summary>
    public IrBlock? ElseBlock { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrIf"/> with the specified condition and blocks.
    /// </summary>
    public IrIf(IrExpression condition, IrBlock thenBlock, IrBlock? elseBlock = null)
    {
        Condition = condition;
        ThenBlock = thenBlock;
        ElseBlock = elseBlock;
    }
}
