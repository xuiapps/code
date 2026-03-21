namespace Xui.GPU.IR;

/// <summary>
/// Represents a block of statements.
/// </summary>
public class IrBlock : IrStatement
{
    public override IrNodeKind Kind => IrNodeKind.Block;
    public List<IrStatement> Statements { get; } = new();

    public IrBlock()
    {
    }

    public IrBlock(IEnumerable<IrStatement> statements)
    {
        Statements.AddRange(statements);
    }
}
