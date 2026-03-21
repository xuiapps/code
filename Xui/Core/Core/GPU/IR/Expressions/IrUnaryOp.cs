namespace Xui.GPU.IR;

/// <summary>
/// Represents a unary operation.
/// </summary>
public class IrUnaryOp : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.UnaryOp;
    public override IrType Type { get; }
    public UnaryOperator Operator { get; }
    public IrExpression Operand { get; }

    public IrUnaryOp(UnaryOperator op, IrExpression operand, IrType type)
    {
        Operator = op;
        Operand = operand;
        Type = type;
    }
}
