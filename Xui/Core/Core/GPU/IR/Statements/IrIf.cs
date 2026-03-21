namespace Xui.GPU.IR;

/// <summary>
/// Represents an if statement.
/// </summary>
public class IrIf : IrStatement
{
    public override IrNodeKind Kind => IrNodeKind.If;
    public IrExpression Condition { get; }
    public IrBlock ThenBlock { get; }
    public IrBlock? ElseBlock { get; }

    public IrIf(IrExpression condition, IrBlock thenBlock, IrBlock? elseBlock = null)
    {
        Condition = condition;
        ThenBlock = thenBlock;
        ElseBlock = elseBlock;
    }
}
