namespace Xui.GPU.IR;

/// <summary>
/// Represents a unary operation.
/// </summary>
public class IrUnaryOp : IrExpression
{
    /// <summary>Gets the node kind for this unary operation.</summary>
    public override IrNodeKind Kind => IrNodeKind.UnaryOp;

    /// <summary>Gets the result type of the unary operation.</summary>
    public override IrType Type { get; }

    /// <summary>Gets the unary operator.</summary>
    public UnaryOperator Operator { get; }

    /// <summary>Gets the operand of the unary operation.</summary>
    public IrExpression Operand { get; }

    /// <summary>Initializes a new instance of the <see cref="IrUnaryOp"/> class.</summary>
    public IrUnaryOp(UnaryOperator op, IrExpression operand, IrType type)
    {
        Operator = op;
        Operand = operand;
        Type = type;
    }
}
