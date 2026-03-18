namespace Xui.GPU.IR;

/// <summary>
/// Base class for all statement nodes in the IR.
/// </summary>
public abstract class IrStatement : IrNode
{
}

/// <summary>
/// Represents a variable declaration.
/// </summary>
public class IrVarDecl : IrStatement
{
    public override IrNodeKind Kind => IrNodeKind.VarDecl;
    public string Name { get; }
    public IrType Type { get; }
    public IrExpression? Initializer { get; }

    public IrVarDecl(string name, IrType type, IrExpression? initializer = null)
    {
        Name = name;
        Type = type;
        Initializer = initializer;
    }
}

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
