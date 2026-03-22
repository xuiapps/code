namespace Xui.GPU.IR;

/// <summary>
/// Represents a binary operation.
/// </summary>
public class IrBinaryOp : IrExpression
{
    /// <summary>Gets the node kind for this binary operation.</summary>
    public override IrNodeKind Kind => IrNodeKind.BinaryOp;

    /// <summary>Gets the result type of the binary operation.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the binary operator.</summary>
    public BinaryOperator Operator { get; }

    /// <summary>Gets the left-hand operand.</summary>
    public IrExpression Left { get; }

    /// <summary>Gets the right-hand operand.</summary>
    public IrExpression Right { get; }

    /// <summary>Initializes a new instance of the <see cref="IrBinaryOp"/> class.</summary>
    public IrBinaryOp(BinaryOperator op, IrExpression left, IrExpression right, IrType type)
    {
        Operator = op;
        Left = left;
        Right = right;
        Type = type;
    }
}
