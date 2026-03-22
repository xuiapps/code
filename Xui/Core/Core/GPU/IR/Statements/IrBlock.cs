namespace Xui.GPU.IR;

/// <summary>
/// Represents a block of statements.
/// </summary>
public class IrBlock : IrStatement
{
    /// <summary>Gets the IR node kind for this block.</summary>
    public override IrNodeKind Kind => IrNodeKind.Block;
    /// <summary>Gets the list of statements in this block.</summary>
    public List<IrStatement> Statements { get; } = new();

    /// <summary>
    /// Initializes a new empty block.
    /// </summary>
    public IrBlock()
    {
    }

    /// <summary>
    /// Initializes a new block with the specified statements.
    /// </summary>
    public IrBlock(IEnumerable<IrStatement> statements)
    {
        Statements.AddRange(statements);
    }
}
